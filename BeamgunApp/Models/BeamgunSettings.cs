using System;

namespace BeamgunApp.Models
{
    public interface IBeamgunSettings
    {
        double DisableTime { get; set; }
    }

    public class BeamgunSettings : IBeamgunSettings
    {
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

        public BeamgunSettings(IDynamicDictionary backing)
        {
            _backing = backing;
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
    }
}
