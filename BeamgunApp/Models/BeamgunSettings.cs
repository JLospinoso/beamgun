using System;
using System.Security.Principal;

namespace BeamgunApp.Models
{
    public interface IBeamgunSettings
    {
        bool IsAdmin { get; }
        double DisableTime { get; set; }
        bool StealFocusEnabled { get; set; }
        bool LockWorkstation { get; set; }
        bool DisableNetworkAdapter { get; set; }
        uint StealFocusInterval { get; set; }
        Version LatestVersion { get; set; }
        string DownloadUrl { get; set; }
        uint UpdatePollInterval { get; set; }
        string VersionUrl { get; set; }
        Guid BeamgunId { get; }
        bool CheckForUpdates { get; set; }
        uint DisableNetworkAdapterInterval { get; set; }
        string GraphicsTheme { get; set; }
    }

    public class BeamgunSettings : IBeamgunSettings
    {

        public uint DisableNetworkAdapterInterval
        {
            get
            {
                return _backing.GetWithDefault(DisableNetworkAdapterIntervalKey, DisableNetworkAdapterIntervalDefault);
            }
            set
            {
                _backing.Set(DisableNetworkAdapterIntervalKey, value);
            }
        }
        public bool CheckForUpdates
        {
            get
            {
                return _backing.GetWithDefault(CheckForUpdatesKey, CheckForUpdatesDefault);
            }
            set
            {
                _backing.Set(CheckForUpdatesKey, value);
            }
        }
        public uint StealFocusInterval
        {
            get
            {
                return _backing.GetWithDefault(StealFocusIntervalKey, StealFocusIntervalDefault);
            }
            set
            {
                _backing.Set(StealFocusIntervalKey, value);
            }
        }
        public string VersionUrl
        {
            get
            {
                return _backing.GetWithDefault(VersionUrlKey, VersionUrlDefault);
            }
            set
            {
                _backing.Set(VersionUrlKey, value);
            }
        }
        public Version LatestVersion
        {
            get
            {
                return new Version(_backing.GetWithDefault(LatestVersionKey, LatestVersionDefault));
            }
            set
            {
                _backing.Set(LatestVersionKey, value);
            }
        }
        public string DownloadUrl
        {
            get
            {
                return _backing.GetWithDefault(DownloadUrlKey, DownloadUrlDefault);
            }
            set
            {
                _backing.Set(DownloadUrlKey, value);
            }
        }
        public uint UpdatePollInterval
        {
            get
            {
                return _backing.GetWithDefault(UpdatePollIntervalKey, UpdatePollIntervalDefault);
            }
            set
            {
                _backing.Set(UpdatePollIntervalKey, value);
            }
        }
        public double DisableTime
        {
            get
            {
                return _backing.GetWithDefault(DisableTimeKey, DisableTimeDefault);
            }
            set
            {
                _backing.Set(DisableTimeKey, value);
            }
        }
        public bool StealFocusEnabled
        {
            get
            {
                return _backing.GetWithDefault(StealFocusEnabledSubkey, StealFocusEnabledDefault);
            }
            set
            {
                _backing.Set(StealFocusEnabledSubkey, value);
            }
        }
        public bool LockWorkstation
        {
            get
            {
                return _backing.GetWithDefault(LockWorkstationSubkey, LockWorkstationDefault);
            }
            set
            {
                _backing.Set(LockWorkstationSubkey, value);
            }
        }
        public bool DisableNetworkAdapter
        {
            get
            {
                return _backing.GetWithDefault(DisableNetworkAdapterSubkey, DisableNetworkAdapterDefault);
            }
            set
            {
                _backing.Set(DisableNetworkAdapterSubkey, value);
            }
        }
        public string GraphicsTheme
        {
            get
            {
                return _backing.GetWithDefault(GraphicsThemeKey, GraphicsThemeDefault);
            }
            set
            {
                _backing.Set(GraphicsThemeKey, value);
            }
        }
        public bool IsAdmin { get; }
        public Guid BeamgunId { get; }

        public BeamgunSettings(IDynamicDictionary backing)
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            _backing = backing;
            BeamgunId = _backing.GetWithDefault(BeamgunIdKey, Guid.NewGuid());
            _backing.Set(BeamgunIdKey, BeamgunId);
        }

        private readonly IDynamicDictionary _backing;
        private const string VersionUrlKey = "VersionUrl";
        private const string VersionUrlDefault = "https://s3.amazonaws.com/net.lospi.beamgun/version.json";
        private const string LatestVersionKey = "LatestVersion";
        private const string LatestVersionDefault = "Unknown";
        private const string DownloadUrlKey = "DownloadUrl";
        private const string DownloadUrlDefault = "Unknown";
        private const string UpdatePollIntervalKey = "UpdatePollInterval";
        private const uint UpdatePollIntervalDefault = 1000 * 60 * 60 * 24;
        private const string DisableTimeKey = "DisableTime";
        private const double DisableTimeDefault = 30;
        private const string StealFocusEnabledSubkey = "StealFocus";
        private const bool StealFocusEnabledDefault = true;
        private const string LockWorkstationSubkey = "LockWorkStation";
        private const bool LockWorkstationDefault = true;
        private const string DisableNetworkAdapterSubkey = "DisableNetworkAdapter";
        private const bool DisableNetworkAdapterDefault = true;
        private const string StealFocusIntervalKey = "StealFocusInterval";
        private const uint StealFocusIntervalDefault = 10;
        private const string BeamgunIdKey = "BeamgunId";
        private const string CheckForUpdatesKey = "CheckForUpdates";
        private const bool CheckForUpdatesDefault = true;
        private const uint DisableNetworkAdapterIntervalDefault = 100;
        private const string DisableNetworkAdapterIntervalKey = "DisableNetworkAdapterInterval";
        private const string GraphicsThemeKey = "GraphicsTheme";
        private const string GraphicsThemeDefault = "DuckHunt";
    }
}
