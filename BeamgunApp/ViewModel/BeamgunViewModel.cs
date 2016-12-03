using System;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Input;
using BeamgunApp.Commands;
using BeamgunApp.Models;

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
            var dictionary = new RegistryBackedDictionary();
            var beamgunSettings = new BeamgunSettings(dictionary);
            BeamgunState = new BeamgunState(beamgunSettings)
            {
                MainWindowVisibility = Visibility.Hidden
            };
            // TODO: This bi-directional relationship feels bad.
            dictionary.BadCastReport += BeamgunState.AppendToAlert;
            BeamgunState.Disabler = new Disabler(BeamgunState);
            BeamgunState.Disabler.Enable(); 
            DisableCommand = new DisableCommand(this, beamgunSettings);
            TrayIconCommand = new TrayIconCommand(this);
            LoseFocusCommand = new DeactivatedCommand(this);
            ResetCommand = new ResetCommand(this);
            ExitCommand = new ExitCommand(this);
            _keystrokeHooker = InstallKeystrokeHooker();
            _usbStorageGuard = InstallUsbStorageGuard(beamgunSettings);
            _alarm = InstallAlarm(beamgunSettings);
            _networkWatcher = new NetworkWatcher(beamgunSettings,
                new NetworkAdapterDisabler(),
                x => BeamgunState.AppendToAlert(x),
                x =>
                {
                    _alarm.Trigger(x);
                    BeamgunState.SetGraphicsLanAlert();
                });
            _keyboardWatcher = new KeyboardWatcher(beamgunSettings, 
                new WorkstationLocker(), 
                x => BeamgunState.AppendToAlert(x),
                x =>
                {
                    _alarm.Trigger(x);
                    BeamgunState.SetGraphicsKeyboardAlert();
                });
            _updateTimer = new VersionCheckerTimer(beamgunSettings, 
                new VersionChecker(), 
                x => BeamgunState.AppendToAlert(x) );
        }
        
        private Alarm InstallAlarm(IBeamgunSettings beamgunSettings)
        {
            var alarm = new Alarm(beamgunSettings.StealFocusInterval, BeamgunState);
            alarm.AlarmCallback += () =>
            {
                BeamgunState.MainWindowState = WindowState.Normal;
                BeamgunState.MainWindowVisibility = Visibility.Visible;
                DoStealFocus();
            };
            return alarm;
        }

        private UsbStorageGuard InstallUsbStorageGuard(IBeamgunSettings beamgunSettings)
        {
            var usbGuard = new UsbStorageGuard(beamgunSettings);
            BeamgunState.UsbMassStorageDisabled = usbGuard.UsbStorageDisabled;
            BeamgunState.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != nameof(BeamgunState.UsbMassStorageDisabled)) return;
                if (!beamgunSettings.IsAdmin)
                {
                    BeamgunState.AppendToAlert("Cannot change USB Mass Storage settings without administrative privileges.");
                }
                try
                {
                    usbGuard.UsbStorageDisabled = BeamgunState.UsbMassStorageDisabled;
                }
                catch (PrivilegeNotHeldException e)
                {
                    BeamgunState.AppendToAlert($"Privileges exception: {e.Message}");
                }
            };
            return usbGuard;
        }

        private KeystrokeHooker InstallKeystrokeHooker()
        {
            var converter = new Models.KeyConverter();
            var keystrokeHooker = new KeystrokeHooker();
            keystrokeHooker.Callback += key =>
            {
                if (!_alarm.Triggered) return;
                BeamgunState.AppendToKeyLog(converter.Convert(key));
            };
            return keystrokeHooker;
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
            _keystrokeHooker?.Dispose();
            _updateTimer?.Dispose();
            _updateTimer?.Dispose();
            _keyboardWatcher?.Dispose();
            _networkWatcher?.Dispose();
            _usbStorageGuard?.Dispose();
        }

        public void Reset()
        {
            BeamgunState.AppendToAlert("Resetting alarm.");
            BeamgunState.Disabler.Enable();
            _alarm.Reset();
            _networkWatcher.Triggered = false;
        }

        private readonly KeystrokeHooker _keystrokeHooker;
        private readonly Alarm _alarm;
        private readonly NetworkWatcher _networkWatcher;
        private readonly UsbStorageGuard _usbStorageGuard;
        private readonly VersionCheckerTimer _updateTimer;
        private readonly KeyboardWatcher _keyboardWatcher;
    }
}
