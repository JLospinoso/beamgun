using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace BeamgunApp.Models
{
    public class UsbMonitor : IDisposable
    {
        public Action DeviceAdded;
        public Action DeviceRemoved;
        public const int DbtDevicearrival = 0x8000; // system detected a new device        
        public const int DbtDeviceremovecomplete = 0x8004; // device is gone      
        public const int WmDevicechange = 0x0219; // device change event      
        private const int DbtDevtypDeviceinterface = 5;
        private readonly Guid _guidDevinterfaceUsbDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
        private readonly IntPtr _notificationHandle;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        private struct DevBroadcastDeviceinterface
        {
            internal int Size;
            internal int DeviceType;
            internal int Reserved;
            internal Guid ClassGuid;
            internal short Name;
        }

        public UsbMonitor(IntPtr windowPtr)
        {
            var dbi = new DevBroadcastDeviceinterface
            {
                DeviceType = DbtDevtypDeviceinterface,
                Reserved = 0,
                ClassGuid = _guidDevinterfaceUsbDevice,
                Name = 0
            };
            dbi.Size = Marshal.SizeOf(dbi);
            var buffer = Marshal.AllocHGlobal(dbi.Size);
            Marshal.StructureToPtr(dbi, buffer, true);
            var source = HwndSource.FromHwnd(windowPtr);
            if (source == null) throw new ArgumentException("Window pointer did not yield valid HWND.");
            var hwndHandle = source.Handle;
            source.AddHook(HwndHandler);
            _notificationHandle = RegisterDeviceNotification(hwndHandle, buffer, 0);
        }

        public void Dispose()
        {
            UnregisterDeviceNotification(_notificationHandle);
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == WmDevicechange)
            {
                var paramAsInt = (int) wparam;
                if (paramAsInt == DbtDeviceremovecomplete)
                {
                    DeviceRemoved?.Invoke();
                }
                else if (paramAsInt == DbtDevicearrival)
                {
                    DeviceAdded?.Invoke();
                }
            }
            handled = false;
            return IntPtr.Zero;
        }
    }
}


