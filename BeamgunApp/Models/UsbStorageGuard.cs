using Microsoft.Win32;

namespace BeamgunApp.Models
{
    public class UsbStorageGuard
    {
        public bool Enabled
        {
            get
            {
                return (int)Registry.GetValue(UsbMassStorageKey, UsbMassStorageSubkey, 3) == 3;
            }
            set
            {
                if (!_settings.IsAdmin) return;
                Registry.SetValue(UsbMassStorageKey, UsbMassStorageSubkey, value ? 3 : 4);
            }
        }

        public UsbStorageGuard(IBeamgunSettings settings)
        {
            _settings = settings;
        }

        private readonly IBeamgunSettings _settings;
        private const string UsbMassStorageKey = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\usbstor";
        private const string UsbMassStorageSubkey = "Start";
    }
}
