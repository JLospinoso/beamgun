using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using BeamgunApp.Annotations;

namespace BeamgunApp.Models
{
    public interface IBeamgunState
    {
        void SetGraphicsArmed();
        void AppendToAlert(string message);
        void SetGraphicsDisabled();
        Visibility MainWindowVisibility { get; set; }
    }

    public class BeamgunState : INotifyPropertyChanged, IBeamgunState
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
        public bool LockOnKeyboard
        {
            get { return _settings.LockOnKeyboard; }
            set
            {
                _settings.LockOnKeyboard = value;
                OnPropertyChanged(nameof(LockOnKeyboard));
            }
        }
        public bool LockOnMouse
        {
            get { return _settings.LockOnMouse; }
            set
            {
                _settings.LockOnMouse = value;
                OnPropertyChanged(nameof(LockOnMouse));
            }
        }
        public bool DisableNetworkAdapter
        {
            get { return _settings.DisableNetworkAdapter && IsAdmin; }
            set
            {
                _settings.DisableNetworkAdapter = value;
                OnPropertyChanged(nameof(DisableNetworkAdapter));
            }
        }
        public bool IsAdmin => _settings.IsAdmin;

        public bool UsbMassStorageDisabled
        {
            get { return _usbMassStorageDisabled; }
            set
            {
                _usbMassStorageDisabled = value;
                OnPropertyChanged(nameof(UsbMassStorageDisabled));
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

        public string VersionInfo
        {
            get { return _versionInfo; }
            set
            {
                _versionInfo = value;
                OnPropertyChanged(nameof(VersionInfo));
            }
        }

        public void SetGraphicsKeyboardAlert()
        {
            TrayIconPath = $"Graphics/{_settings.GraphicsTheme}/KeyboardAlertIcon.ico";
            BannerPath = $"Graphics/{_settings.GraphicsTheme}/BeamgunAppBannerKeyboardAlert.png";
        }

        public void SetGraphicsMouseAlert()
        {
            TrayIconPath = $"Graphics/{_settings.GraphicsTheme}/MouseAlertIcon.ico";
            BannerPath = $"Graphics/{_settings.GraphicsTheme}/BeamgunAppBannerMouseAlert.png";
        }

        public void SetGraphicsLanAlert()
        {
            TrayIconPath = $"Graphics/{_settings.GraphicsTheme}/LanAlertIcon.ico";
            BannerPath = $"Graphics/{_settings.GraphicsTheme}/BeamgunAppBannerLanAlert.png";
        }

        public void SetGraphicsDisabled()
        {
            TrayIconPath = $"Graphics/{_settings.GraphicsTheme}/DisabledIcon.ico";
            BannerPath = $"Graphics/{_settings.GraphicsTheme}/BeamgunAppBannerDisabled.png";
        }

        public void SetGraphicsArmed()
        {
            TrayIconPath = $"Graphics/{_settings.GraphicsTheme}/ArmedIcon.ico";
            BannerPath = $"Graphics/{_settings.GraphicsTheme}/BeamgunAppBannerArmed.png";
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

        public BeamgunState(IBeamgunSettings settings)
        {
            _settings = settings;
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

        private readonly IBeamgunSettings _settings;
        private string _alertLog = "";
        private string _trayIconPath;
        private string _bannerPath;
        private string _versionInfo = $"Beamgun v{Assembly.GetExecutingAssembly().GetName().Version}. Copyright © 2016-2018 Josh Lospinoso";
        private WindowState _mainWindowState;
        private Visibility _mainWindowVisibility;
        private bool _usbMassStorageDisabled;
        private bool _lastAlertWasKeystroke;
    }
}
