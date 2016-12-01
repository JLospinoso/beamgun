using Microsoft.Win32;

namespace BeamgunApp.Models
{
    public class UsbStorageGuard
    {
        public bool UsbStorageDisabled
        {
            get
            {
                return (int)Registry.GetValue(UsbMassStorageKey, UsbMassStorageSubkey, 4) == 4;
            }
            set
            {
                if (!_settings.IsAdmin) return;
                Registry.SetValue(UsbMassStorageKey, UsbMassStorageSubkey, value ? 4 : 3);
            }
        }

        public UsbStorageGuard(IBeamgunSettings settings)
        {
            _settings = settings;
        }

        public void Dispose()
        {
            //NOP
        }

        private readonly IBeamgunSettings _settings;
        private const string UsbMassStorageKey = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\usbstor";
        private const string UsbMassStorageSubkey = "Start";
    }
}
