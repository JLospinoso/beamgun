using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BeamgunApp.Annotations;
using Microsoft.Win32;

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
        
        public string KeyLog
        {
            get
            {
                return _keyLog; ;
            }
            set
            {
                _keyLog = value;
                OnPropertyChanged(nameof(KeyLog));
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
                Registry.SetValue(BaseKey, StealFocusSubkey, _stealFocus);
            }
        }

        public bool LockWorkStation
        {
            get { return _lockWorkStation; }
            set
            {
                _lockWorkStation = value;
                OnPropertyChanged(nameof(LockWorkStation));
                Registry.SetValue(BaseKey, LockWorkstationSubkey, _lockWorkStation);
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

        public void SetGraphicsAlert()
        {
            TrayIconPath = "Graphics/AlertIcon.ico";
            BannerPath = "Graphics/BeamgunAppBannerAlert.png";
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
            AlertLog += $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} {input}\n";
        }
        
        public void AppendToKeyLog(string input)
        {
            KeyLog += input;
        }

        public BeamgunState()
        {
            StealFocus = (string)Registry.GetValue(BaseKey, StealFocusSubkey, "True") == "True";
            LockWorkStation = (string)Registry.GetValue(BaseKey, LockWorkstationSubkey, "False") == "True";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _alertLog = "";
        private string _keyLog = "";
        private string _trayIconPath;
        private string _bannerPath;
        private WindowState _mainWindowState;
        private Visibility _mainWindowVisibility;
        private bool _stealFocus;
        private bool _lockWorkStation;
        private const string BaseKey = "HKEY_CURRENT_USER\\SOFTWARE\\Beamgun";
        private const string StealFocusSubkey = "StealFocus";
        private const string LockWorkstationSubkey = "LockWorkStation";
    }
}
