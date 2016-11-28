using System;
using System.Management;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BeamgunApp.Commands;
using BeamgunApp.Models;
using KeyConverter = BeamgunApp.Models.KeyConverter;

namespace BeamgunApp.ViewModel
{
    public interface IViewModel
    {
        bool IsVisible { get; set; }
        void DoStealFocus();
        void Reset();
        void DisableUntil(DateTime minutes);
    }

    public class BeamgunViewModel : IDisposable, IViewModel
    {
        public BeamgunState BeamgunState { get; }
        public ICommand DisableCommand { get; }
        public ICommand TrayIconCommand { get; }
        public ICommand LoseFocusCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ExitCommand { get; }
        public Action StealFocus { get; set; }

        public bool IsVisible
        {
            get
            {
                return BeamgunState.MainWindowVisibility == Visibility.Visible;
            }
            set
            {
                BeamgunState.MainWindowVisibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public BeamgunViewModel()
        {
            _beamgunSettings = new BeamgunSettings(new RegistryBackedDictionary());
            BeamgunState = new BeamgunState
            {
                MainWindowVisibility = Visibility.Hidden
            };
            BeamgunState.Disabler = new Disabler(BeamgunState);
            BeamgunState.Disabler.Enable();
            DisableCommand = new DisableCommand(this, _beamgunSettings);
            TrayIconCommand = new TrayIconCommand(this);
            LoseFocusCommand = new DeactivatedCommand(this);
            ResetCommand = new ResetCommand(this);
            ExitCommand = new ExitCommand(this);

            const uint repeatInterval = 10;
            var converter = new KeyConverter();
            _keystrokeHooker = new KeystrokeHooker();

            _alarm = new Alarm(repeatInterval, BeamgunState);
            _alarm.AlarmCallback += () =>
            {
                BeamgunState.MainWindowState = WindowState.Normal;
                BeamgunState.MainWindowVisibility = Visibility.Visible;
                DoStealFocus();
            };
            var workstationLocker = new WorkstationLocker();
            
            _keystrokeHooker.Callback += key =>
            {
                if (!_alarm.Triggered) return;
                BeamgunState.AppendToKeyLog(converter.Convert(key));
            };

            var networkQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_NetworkAdapter\"");
            _networkWatcher = new ManagementEventWatcher(networkQuery);
            _networkWatcher.EventArrived += (caller, args) =>
            {
                BeamgunState.SetGraphicsLanAlert();
                var obj = (ManagementBaseObject)args.NewEvent["TargetInstance"];
                var alertMessage = $"Alerting on network adapter insertion: " +
                   $"{obj["AdapterType"]} " +
                   $"{obj["Caption"]} " +
                   $"{obj["Description"]} " +
                   $"{obj["DeviceID"]} " +
                   $"{obj["GUID"]} " +
                   $"{obj["MACAddress"]} " +
                   $"{obj["Manufacturer"]} " +
                   $"{obj["Name"]} " +
                   $"{obj["PermanentAddress"]} " +
                   $"{obj["NetworkAddresses"]} " +
                   $"{obj["ProductName"]} " +
                   $"{obj["ServiceName"]} " +
                   $"{obj["SystemCreationClassName"]} " +
                   $"{obj["SystemName"]} ";
                _alarm.Trigger(alertMessage);
                BeamgunState.AppendToAlert(alertMessage);
                if (!BeamgunState.DisableNetworkAdapter) return;
                var query = $"SELECT * FROM Win32_NetworkAdapter WHERE DeviceID = \"{obj["DeviceID"]}\"";
                var searcher = new ManagementObjectSearcher(query);
                foreach (var item in searcher.Get())
                {
                    var managementObject = (ManagementObject)item;
                    try
                    {
                        var disableCode = (uint)managementObject.InvokeMethod("Disable", null);
                        BeamgunState.AppendToAlert(disableCode == 0
                            ? "Network adapter successfully disabled."
                            : $"Danger! Unable to disable network adapter: {disableCode}");
                        return;
                    }
                    catch (ManagementException e) 
                    {
                        BeamgunState.AppendToAlert($"Error disabling new network adapter: {e}");
                    }
                }
            };
            _networkWatcher.Start();
            
            var keyboardQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Keyboard\"");
            _keyboardWatcher = new ManagementEventWatcher(keyboardQuery);
            _keyboardWatcher.EventArrived += (caller, args) =>
            {
                BeamgunState.SetGraphicsKeyboardAlert();
                var obj = (ManagementBaseObject)args.NewEvent["TargetInstance"];
                var alertMessage = $"Alerting on keyboard insertion: " +
                   $"{obj["Name"]} " +
                   $"{obj["Caption"]} " +
                   $"{obj["Description"]} " +
                   $"{obj["DeviceID"]} " +
                   $"{obj["Layout"]} " +
                   $"{obj["PNPDeviceID"]}.";
                _alarm.Trigger(alertMessage);
                if (!BeamgunState.LockWorkStation) return;
                BeamgunState.AppendToAlert(workstationLocker.Lock()
                    ? "Successfully locked the workstation." 
                    : "Could not lock the workstation.");
            };
            _keyboardWatcher.Start();

            _versionChecker = new VersionChecker();
            _updateTimer = new Timer(state =>
            {
                try
                {
                    _versionChecker.Update(_beamgunSettings);
                    var availableVersion = _beamgunSettings.LatestVersion;
                    var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    BeamgunState.AppendToAlert(
                        availableVersion > currentVersion
                            ? $"Version {availableVersion} is available at {_beamgunSettings.DownloadUrl}"
                            : $"Beamgun is up to date.");
                }
                catch (Exception e)
                {
                    BeamgunState.AppendToAlert($"Unable to connect to update server. {e.Message}");
                }
            }, null, 0, _beamgunSettings.UpdatePollInterval);
        }

        public void DoStealFocus()
        {
            if (BeamgunState.StealFocus)
            {
                StealFocus();
            }
        }

        public void DisableUntil(DateTime time)
        {
            BeamgunState.Disabler.DisableUntil(time);
        }
        
        public void Dispose()
        {
            _keystrokeHooker.Dispose();
            _networkWatcher.Stop();
            _keyboardWatcher.Stop();
            _updateTimer.Dispose();
        }

        public void Reset()
        {
            BeamgunState.AppendToAlert("Resetting alarm.");
            BeamgunState.Disabler.Enable();
            _alarm.Reset();
        }
        
        private readonly Timer _updateTimer;
        private readonly KeystrokeHooker _keystrokeHooker;
        private readonly Alarm _alarm;
        private readonly ManagementEventWatcher _networkWatcher, _keyboardWatcher;
        private readonly BeamgunSettings _beamgunSettings;
        private readonly VersionChecker _versionChecker;
    }
}
