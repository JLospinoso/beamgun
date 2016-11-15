using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using BeamgunApp.Commands;
using BeamgunApp.Models;
using KeyConverter = BeamgunApp.Models.KeyConverter;

namespace BeamgunApp.ViewModel
{
    public class BeamgunViewModel : IDisposable
    {
        public BeamgunState BeamgunState { get; }
        public ICommand DisableCommand { get; }
        public ICommand TrayIconCommand { get; }
        public ICommand LoseFocusCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ExitCommand { get; }
        public Action StealFocus { get; set; }

        public BeamgunViewModel(Window mainWindow)
        {
            BeamgunState = new BeamgunState
            {
                MainWindowVisibility = Visibility.Hidden
            };
            BeamgunState.Disabler = new Disabler(BeamgunState);
            BeamgunState.Disabler.Enable();
            DisableCommand = new DisableCommand(this);
            TrayIconCommand = new TrayIconCommand(this);
            LoseFocusCommand = new DeactivatedCommand(this);
            ResetCommand = new ResetCommand(this);
            ExitCommand = new ExitCommand(this);
            _workstationLocker = new WorkstationLocker();

            const uint repeatInterval = 10;
            var converter = new KeyConverter();
            _keystrokeHooker = new KeystrokeHooker();

            _alarm = new Alarm(repeatInterval, BeamgunState);
            _alarm.AlarmCallback += () =>
            {
                BeamgunState.MainWindowState = WindowState.Normal;
                BeamgunState.MainWindowVisibility = Visibility.Visible;
                BeamgunState.SetGraphicsAlert();
                if (BeamgunState.StealFocus)
                {
                    StealFocus();
                }
            };

            var windowHandle = new WindowInteropHelper(mainWindow).Handle;
            _usbMonitor = new UsbMonitor(windowHandle);
            _usbMonitor.DeviceAdded += () =>
            {
                BeamgunState.AppendToAlert("USB device inserted.");
                _alarm.Trigger("Alerting on USB device insertion.");
                if (!BeamgunState.LockWorkStation) return;
                var result = _workstationLocker.Lock();
                var message = result ? "Successfully locked the workstation." : "Could not lock the workstation.";
                BeamgunState.AppendToAlert(message);
            };
            _usbMonitor.DeviceRemoved += () =>
            {
                BeamgunState.AppendToAlert("USB device removed.");
            };
            
            _keystrokeHooker.Callback += key =>
            {
                if (!_alarm.Triggered) return;
                BeamgunState.AppendToKeyLog(converter.Convert(key));
            };
        }
        
        public void DisableUntil(DateTime time)
        {
            BeamgunState.Disabler.DisableUntil(time);
        }
        
        public void Dispose()
        {
            _keystrokeHooker.Dispose();
            _usbMonitor.Dispose();
        }

        public void Reset()
        {
            BeamgunState.AppendToAlert("Resetting alarm.");
            BeamgunState.Disabler.Enable();
            BeamgunState.KeyLog = $"Reset at {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}\n";
            _alarm.Reset();
        }
        
        private readonly KeystrokeHooker _keystrokeHooker;
        private readonly UsbMonitor _usbMonitor;
        private readonly Alarm _alarm;
        private readonly WorkstationLocker _workstationLocker;
    }
}
