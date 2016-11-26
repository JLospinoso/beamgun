using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BeamgunApp.Annotations;
using Microsoft.Win32;
using System.Security.Principal;

namespace BeamgunApp.Models
{
    public class BeamgunState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Disabler Disabler { get; set; }

        public string TrayIconPath
        {
            get { return _trayIconPath; }
            set
            {
                _trayIconPath = value;
                OnPropertyChanged(nameof(TrayIconPath));
            }
        }

        public string AlertLog
        {
            get
            {
                return _alertLog;;
            }
            set
            {
                _alertLog = value;
                OnPropertyChanged(nameof(AlertLog));
            }
        }
        
        public WindowState MainWindowState
        {
            get { return _mainWindowState; }
            set
            {
                _mainWindowState = value;
                OnPropertyChanged(nameof(MainWindowState));
            }
        }
        
        public Visibility MainWindowVisibility
        {
            get { return _mainWindowVisibility; }
            set
            {
                _mainWindowVisibility = value;
                OnPropertyChanged(nameof(MainWindowVisibility));
            }
        }
        
        public bool StealFocus
        {
            get { return _stealFocus; }
            set
            {
                _stealFocus = value;
                OnPropertyChanged(nameof(StealFocus));
                Registry.SetValue(BeamgunBaseKey, StealFocusSubkey, _stealFocus);
            }
        }
        
        public bool LockWorkStation
        {
            get { return _lockWorkStation; }
            set
            {
                _lockWorkStation = value;
                OnPropertyChanged(nameof(LockWorkStation));
                Registry.SetValue(BeamgunBaseKey, LockWorkstationSubkey, _lockWorkStation);
            }
        }

        public bool DisableNetworkAdapter
        {
            get { return _disableNetworkAdapter && IsAdmin; }
            set
            {
                _disableNetworkAdapter = value;
                OnPropertyChanged(nameof(DisableNetworkAdapter));
                Registry.SetValue(BeamgunBaseKey, DisableNetworkAdapterSubkey, _disableNetworkAdapter);
            }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public bool UsbMassStorageEnabled
        {
            get { return _usbMassStorageEnabled; }
            set
            {
                _usbMassStorageEnabled = value;
                OnPropertyChanged(nameof(UsbMassStorageEnabled));
                if (!IsAdmin) return;
                try
                {
                    Registry.SetValue(UsbMassStorageKey, UsbMassStorageSubkey, value ? 3 : 4);
                }
                catch (UnauthorizedAccessException e)
                {
                    AppendToAlert($"Unauthorized access: {e.Message}");
                }
            }
        }

        public string BannerPath
        {
            get { return _bannerPath; }
            set
            {
                _bannerPath = value;
                OnPropertyChanged(nameof(BannerPath));
            }
        }

        public void SetGraphicsKeyboardAlert()
        {
            TrayIconPath = "Graphics/KeyboardAlertIcon.ico";
            BannerPath = "Graphics/BeamgunAppBannerKeyboardAlert.png";
        }

        public void SetGraphicsLanAlert()
        {
            TrayIconPath = "Graphics/LanAlertIcon.ico";
            BannerPath = "Graphics/BeamgunAppBannerLanAlert.png";
        }

        public void SetGraphicsDisabled()
        {
            TrayIconPath = "Graphics/DisabledIcon.ico";
            BannerPath = "Graphics/BeamgunAppBannerDisabled.png";
        }

        public void SetGraphicsArmed()
        {
            TrayIconPath = "Graphics/ArmedIcon.ico";
            BannerPath = "Graphics/BeamgunAppBannerArmed.png";
        }
        
        public void AppendToAlert(string input)
        {
            if (_lastAlertWasKeystroke)
            {
                AlertLog += "\n";
            }
            AlertLog += $"{GetDateTimeString()}: {input}\n";
            _lastAlertWasKeystroke = false;
        }
        
        public void AppendToKeyLog(string input)
        {
            AlertLog += _lastAlertWasKeystroke ? input : $"{GetDateTimeString()}: {input}";
            _lastAlertWasKeystroke = true;
        }

        public BeamgunState()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            DisableNetworkAdapter = (string)Registry.GetValue(BeamgunBaseKey, DisableNetworkAdapterSubkey, "True") == "True";
            StealFocus = (string)Registry.GetValue(BeamgunBaseKey, StealFocusSubkey, "True") == "True";
            LockWorkStation = (string)Registry.GetValue(BeamgunBaseKey, LockWorkstationSubkey, "False") == "True";
            UsbMassStorageEnabled = (int)Registry.GetValue(UsbMassStorageKey, UsbMassStorageSubkey, 3) == 3;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string GetDateTimeString()
        {
            return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
        }

        private string _alertLog = "";
        private string _trayIconPath;
        private string _bannerPath;
        private WindowState _mainWindowState;
        private Visibility _mainWindowVisibility;
        private bool _stealFocus;
        private bool _lockWorkStation;
        private bool _usbMassStorageEnabled;
        private bool _disableNetworkAdapter;
        private bool _isAdmin;
        private bool _lastAlertWasKeystroke;
        private const string UsbMassStorageKey = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\usbstor";
        private const string UsbMassStorageSubkey = "Start";
        private const string BeamgunBaseKey = "HKEY_CURRENT_USER\\SOFTWARE\\Beamgun";
        private const string StealFocusSubkey = "StealFocus";
        private const string LockWorkstationSubkey = "LockWorkStation";
        private const string DisableNetworkAdapterSubkey = "DisableNetworkAdapter";
    }
}
