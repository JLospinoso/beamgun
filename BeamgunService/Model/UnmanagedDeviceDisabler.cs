using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeamgunService.Model
{
    /*
const GUID KEYBOARD_CLASS_GUID = { 0x4D36E96B, 0xE325, 0x11CE, { 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18 } };
auto devs = SetupDiGetClassDevs(&KEYBOARD_CLASS_GUID, NULL, 0, DICGF_PRESENT);
auto devCount = 0;
SP_DEVINFO_DATA devInfo;
devInfo.cbSize = sizeof(SP_DEV_INFO_DATA);
auto enumeratingDevices = true;
while (enumeratingDevices) {
	enumeratingDevices = SetupDiEnumDeviceInfo(devs, devCount, &devInfo);
	if (enumeratingDevices) {
		auto res = SetupDiRemoveDevice(devs, &devInfo);
		devCount++;
	}
}
SetupDiDestroyDeviceInfoList(devs);
     */
    [StructLayout(LayoutKind.Sequential)]
    struct SP_DEVICE_INTERFACE_DATA
    {
        public Int32 cbSize;
        public Guid interfaceClassGuid;
        public Int32 flags;
        private UIntPtr reserved;
    }

    // Device Property
    [StructLayout(LayoutKind.Sequential)]
    struct DEVPROPKEY //TODO: Was marked unsafe
    {
        public Guid fmtid;
        public UInt32 pid;
    }

    public class UnmanagedDeviceDisabler
    {
        private const int DIGCF_DEFAULT = 0x1;
        private const int DIGCF_PRESENT = 0x2;
        private const int DIGCF_ALLCLASSES = 0x4;
        private const int DIGCF_PROFILE = 0x8;
        private const int DIGCF_DEVICEINTERFACE = 0x10;
        private const Int32 INVALID_HANDLE_VALUE = -1;
        private Guid _guidKeyboard = new Guid(0x4D36E96B, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, uint flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiRemoveDevice(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiGetDevicePropertyW( //TODO: Was marked unsafe
            IntPtr deviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA deviceInfoData,
            ref DEVPROPKEY propertyKey,
            out UInt64 propertyType,
            IntPtr propertyBuffer,
            Int32 propertyBufferSize,
            out int requiredSize,
            UInt32 flags);

        public void Foo()
        {
            var diskGuid = new Guid(_guidKeyboard.ToByteArray());
            var h = SetupDiGetClassDevs(ref diskGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT);
            if (h != (IntPtr)INVALID_HANDLE_VALUE)
            {
                var enumeratingDevices = true;
                uint i = 0;
                while (enumeratingDevices)
                {
                    var dia = new SP_DEVICE_INTERFACE_DATA();
                    dia.cbSize = Marshal.SizeOf(dia);
                    enumeratingDevices = SetupDiEnumDeviceInfo(h, i, ref dia);
                    if (enumeratingDevices)
                    {
                        Console.WriteLine($"[ ] HardDisabler found device: {dia.interfaceClassGuid}");
                        var removeResult = false; //UNSAFE: SetupDiRemoveDevice(h, ref dia);
                        Console.WriteLine(removeResult ?
                            $"[+] Removed." :
                            $"[-] Unable to remove.");
                    }
                    i++;
                }
            }
            SetupDiDestroyDeviceInfoList(h);
        }

    }

    public static class DisableHardware
    {
        const uint DIF_PROPERTYCHANGE = 0x12;
        const uint DICS_ENABLE = 1;
        const uint DICS_DISABLE = 2;  // disable device
        const uint DICS_FLAG_GLOBAL = 1; // not profile-specific
        const uint DIGCF_ALLCLASSES = 4;
        const uint DIGCF_PRESENT = 2;
        const uint ERROR_INVALID_DATA = 13;
        const uint ERROR_NO_MORE_ITEMS = 259;
        const uint ERROR_ELEMENT_NOT_FOUND = 1168;

        static DEVPROPKEY DEVPKEY_Device_DeviceDesc;
        static DEVPROPKEY DEVPKEY_Device_HardwareIds;

        [StructLayout(LayoutKind.Sequential)]
        struct SP_CLASSINSTALL_HEADER
        {
            public UInt32 cbSize;
            public UInt32 InstallFunction;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_PROPCHANGE_PARAMS
        {
            public SP_CLASSINSTALL_HEADER ClassInstallHeader;
            public UInt32 StateChange;
            public UInt32 Scope;
            public UInt32 HwProfile;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            public UInt32 cbSize;
            public Guid classGuid;
            public UInt32 devInst;
            public IntPtr reserved;     // CHANGE #1 - was UInt32
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DEVPROPKEY
        {
            public Guid fmtid;
            public UInt32 pid;
        }

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevsW(
            [In] ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPWStr)]
string Enumerator,
            IntPtr parent,
            UInt32 flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiDestroyDeviceInfoList(IntPtr handle);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet,
            UInt32 memberIndex,
            [Out] out SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiSetClassInstallParams(
            IntPtr deviceInfoSet,
            [In] ref SP_DEVINFO_DATA deviceInfoData,
            [In] ref SP_PROPCHANGE_PARAMS classInstallParams,
            UInt32 ClassInstallParamsSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiChangeState(
            IntPtr deviceInfoSet,
            [In] ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiGetDevicePropertyW(
                IntPtr deviceInfoSet,
                [In] ref SP_DEVINFO_DATA DeviceInfoData,
                [In] ref DEVPROPKEY propertyKey,
                [Out] out UInt32 propertyType,
                IntPtr propertyBuffer,
                UInt32 propertyBufferSize,
                out UInt32 requiredSize,
                UInt32 flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiGetDeviceRegistryPropertyW(
          IntPtr DeviceInfoSet,
          [In] ref SP_DEVINFO_DATA DeviceInfoData,
          UInt32 Property,
          [Out] out UInt32 PropertyRegDataType,
          IntPtr PropertyBuffer,
          UInt32 PropertyBufferSize,
          [In, Out] ref UInt32 RequiredSize
        );

        static DisableHardware()
        {
            DisableHardware.DEVPKEY_Device_DeviceDesc = new DEVPROPKEY();
            DEVPKEY_Device_DeviceDesc.fmtid = new Guid(
                    0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67,
                    0xd1, 0x46, 0xa8, 0x50, 0xe0);
            DEVPKEY_Device_DeviceDesc.pid = 2;

            DEVPKEY_Device_HardwareIds = new DEVPROPKEY();
            DEVPKEY_Device_HardwareIds.fmtid = new Guid(
                0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67,
                0xd1, 0x46, 0xa8, 0x50, 0xe0);
            DEVPKEY_Device_HardwareIds.pid = 3;
        }

        // http://stackoverflow.com/questions/4097000/how-do-i-disable-a-system-device-programatically
        public static void DisableDevice(Func<Dictionary<SetupDiGetDeviceRegistryPropertyEnum, string>, bool> filter, bool disable = true)
        {
            IntPtr info = IntPtr.Zero;
            Guid NullGuid = Guid.Empty;
            try
            {
                info = SetupDiGetClassDevsW(
                    ref NullGuid,
                    null,
                    IntPtr.Zero,
                    DIGCF_ALLCLASSES);
                CheckError("SetupDiGetClassDevs");

                SP_DEVINFO_DATA devdata = new SP_DEVINFO_DATA();
                devdata.cbSize = (UInt32)Marshal.SizeOf(devdata);

                // Get first device matching device criterion.
                for (uint i = 0; ; i++)
                {
                    SetupDiEnumDeviceInfo(info,
                        i,
                        out devdata);
                    // if no items match filter, throw
                    if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_ITEMS)
                        CheckError("No device found matching filter.", 0xcffff);
                    CheckError("SetupDiEnumDeviceInfo");

                    //var devicepath = GetStringPropertyForDevice(info, devdata, 1); // SPDRP_HARDWAREID
                    var map = new Dictionary<SetupDiGetDeviceRegistryPropertyEnum, string>();
                    uint enumIndex = 0;
                    foreach (SetupDiGetDeviceRegistryPropertyEnum x in Enum.GetValues(typeof(SetupDiGetDeviceRegistryPropertyEnum)))
                    {
                        map[x] = GetStringPropertyForDevice(info, devdata, enumIndex++);
                    }
                    //var devicepath = GetStringPropertyForDevice(info, devdata, 1);

                    // Uncomment to print name/path
                    //Console.WriteLine(GetStringPropertyForDevice(info,
                    //                         devdata, DEVPKEY_Device_DeviceDesc));
                    //Console.WriteLine("   {0}", devicepath);
                    if (filter(map)) break;
                }

                SP_CLASSINSTALL_HEADER header = new SP_CLASSINSTALL_HEADER();
                header.cbSize = (UInt32)Marshal.SizeOf(header);
                header.InstallFunction = DIF_PROPERTYCHANGE;

                SP_PROPCHANGE_PARAMS propchangeparams = new SP_PROPCHANGE_PARAMS();
                propchangeparams.ClassInstallHeader = header;
                propchangeparams.StateChange = disable ? DICS_DISABLE : DICS_ENABLE;
                propchangeparams.Scope = DICS_FLAG_GLOBAL;
                propchangeparams.HwProfile = 0;

                SetupDiSetClassInstallParams(info,
                    ref devdata,
                    ref propchangeparams,
                    (UInt32)Marshal.SizeOf(propchangeparams));
                CheckError("SetupDiSetClassInstallParams");

                SetupDiChangeState(
                    info,
                    ref devdata);
                CheckError("SetupDiChangeState");
            }
            finally
            {
                if (info != IntPtr.Zero)
                    SetupDiDestroyDeviceInfoList(info);
            }
        }
        private static void CheckError(string message, int lasterror = -1)
        {

            int code = lasterror == -1 ? Marshal.GetLastWin32Error() : lasterror;
            if (code != 0)
                throw new ApplicationException(
                    String.Format("Error disabling hardware device (Code {0}): {1}",
                        code, message));
        }

        private static string GetStringPropertyForDevice(IntPtr info, SP_DEVINFO_DATA devdata,
            uint propId)
        {
            uint proptype, outsize;
            IntPtr buffer = IntPtr.Zero;
            try
            {
                uint buflen = 512;
                buffer = Marshal.AllocHGlobal((int)buflen);
                outsize = 0;
                // CHANGE #2 - Use this instead of SetupDiGetDeviceProperty 
                SetupDiGetDeviceRegistryPropertyW(
                    info,
                    ref devdata,
                    propId,
                    out proptype,
                    buffer,
                    buflen,
                    ref outsize);
                byte[] lbuffer = new byte[outsize];
                Marshal.Copy(buffer, lbuffer, 0, (int)outsize);
                int errcode = Marshal.GetLastWin32Error();
                if (errcode == ERROR_INVALID_DATA) return null;
                CheckError("SetupDiGetDeviceProperty", errcode);
                return Encoding.Unicode.GetString(lbuffer);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
        }

        public enum SetupDiGetDeviceRegistryPropertyEnum : uint
        {
            SPDRP_DEVICEDESC = 0x00000000, // DeviceDesc (R/W)
            SPDRP_HARDWAREID = 0x00000001, // HardwareID (R/W)
            SPDRP_COMPATIBLEIDS = 0x00000002, // CompatibleIDs (R/W)
            SPDRP_UNUSED0 = 0x00000003, // unused
            SPDRP_SERVICE = 0x00000004, // Service (R/W)
            SPDRP_UNUSED1 = 0x00000005, // unused
            SPDRP_UNUSED2 = 0x00000006, // unused
            SPDRP_CLASS = 0x00000007, // Class (R--tied to ClassGUID)
            SPDRP_CLASSGUID = 0x00000008, // ClassGUID (R/W)
            SPDRP_DRIVER = 0x00000009, // Driver (R/W)
            SPDRP_CONFIGFLAGS = 0x0000000A, // ConfigFlags (R/W)
            SPDRP_MFG = 0x0000000B, // Mfg (R/W)
            SPDRP_FRIENDLYNAME = 0x0000000C, // FriendlyName (R/W)
            SPDRP_LOCATION_INFORMATION = 0x0000000D, // LocationInformation (R/W)
            SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E, // PhysicalDeviceObjectName (R)
            SPDRP_CAPABILITIES = 0x0000000F, // Capabilities (R)
            SPDRP_UI_NUMBER = 0x00000010, // UiNumber (R)
            SPDRP_UPPERFILTERS = 0x00000011, // UpperFilters (R/W)
            SPDRP_LOWERFILTERS = 0x00000012, // LowerFilters (R/W)
            SPDRP_BUSTYPEGUID = 0x00000013, // BusTypeGUID (R)
            SPDRP_LEGACYBUSTYPE = 0x00000014, // LegacyBusType (R)
            SPDRP_BUSNUMBER = 0x00000015, // BusNumber (R)
            SPDRP_ENUMERATOR_NAME = 0x00000016, // Enumerator Name (R)
            SPDRP_SECURITY = 0x00000017, // Security (R/W, binary form)
            SPDRP_SECURITY_SDS = 0x00000018, // Security (W, SDS form)
            SPDRP_DEVTYPE = 0x00000019, // Device Type (R/W)
            SPDRP_EXCLUSIVE = 0x0000001A, // Device is exclusive-access (R/W)
            SPDRP_CHARACTERISTICS = 0x0000001B, // Device Characteristics (R/W)
            SPDRP_ADDRESS = 0x0000001C, // Device Address (R)
            SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D, // UiNumberDescFormat (R/W)
            SPDRP_DEVICE_POWER_DATA = 0x0000001E, // Device Power Data (R)
            SPDRP_REMOVAL_POLICY = 0x0000001F, // Removal Policy (R)
            SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020, // Hardware Removal Policy (R)
            SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021, // Removal Policy Override (RW)
            SPDRP_INSTALL_STATE = 0x00000022, // Device Install State (R)
            SPDRP_LOCATION_PATHS = 0x00000023, // Device Location Paths (R)
            SPDRP_BASE_CONTAINERID = 0x00000024  // Base ContainerID (R)
        }

    }
}
