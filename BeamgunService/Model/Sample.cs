using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeamgunService.Model
{
    //From http://stackoverflow.com/questions/20174169/is-it-possible-to-get-the-pnpdeviceid-of-a-network-adapter-without-using-wmi
    class NetPnP
    {
        internal static Guid GUID_DEVINTERFACE_NET = new Guid("CAC88484-7515-4C03-82E6-71A87ABAC361");

        public static string GetPnPDeviceId(NetworkInterface ni)
        {
            Guid classGuid = GUID_DEVINTERFACE_NET;
            IntPtr hwndParent = IntPtr.Zero;
            Int32 flags = Win32DeviceMgmt.DIGCF_DEVICEINTERFACE | Win32DeviceMgmt.DIGCF_PRESENT;
            IntPtr pDevInfoSet = IntPtr.Zero;
            IntPtr pNewDevInfoSet = IntPtr.Zero;
            try
            {
                pNewDevInfoSet = Win32DeviceMgmt.SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, hwndParent, flags);//, pDevInfoSet, strMachineName, IntPtr.Zero);
                if (pNewDevInfoSet == IntPtr.Zero)
                {
                    //Logger.Log("Failed to get device information list");
                    return "";
                }

                Int32 iRet;
                Int32 iMemberIndex = 0;
                do
                {
                    Win32DeviceMgmt.SP_DEVINFO_DATA devInfoData = new Win32DeviceMgmt.SP_DEVINFO_DATA();
                    devInfoData.ClassGuid = Guid.Empty;
                    devInfoData.DevInst = 0;
                    devInfoData.Reserved = UIntPtr.Zero;
                    devInfoData.cbSize = Marshal.SizeOf(devInfoData);

                    iRet = Win32DeviceMgmt.SetupDiEnumDeviceInfo(pNewDevInfoSet, iMemberIndex, ref devInfoData);
                    if (iRet == 0)
                    {
                        Int32 iLastError = Win32DeviceMgmt.GetLastError();
                        if (iLastError == Win32DeviceMgmt.ERROR_NO_MORE_FILES)
                        {
                            //Console.WriteLine("No more devices in list");
                            break;
                        }
                        else
                        {
                            iMemberIndex++;
                            continue;
                        }
                    }
                    string desc = GetDevicePropertyString(pNewDevInfoSet, devInfoData, SetupDiGetDeviceRegistryPropertyEnum.SPDRP_DEVICEDESC);
                    if (ni.Description.Equals(desc)) return GetDeviceInstanceId(pNewDevInfoSet, devInfoData);
                    string friendly = GetDevicePropertyString(pNewDevInfoSet, devInfoData, SetupDiGetDeviceRegistryPropertyEnum.SPDRP_FRIENDLYNAME);
                    if (ni.Description.Equals(friendly)) return GetDeviceInstanceId(pNewDevInfoSet, devInfoData);

                    iMemberIndex++;
                } while (true);
                return "";
            }
            finally
            {
                Win32DeviceMgmt.SetupDiDestroyDeviceInfoList(pNewDevInfoSet);
            }
        }

        static String GetDeviceInstanceId(IntPtr DeviceInfoSet, Win32DeviceMgmt.SP_DEVINFO_DATA DeviceInfoData)
        {
            StringBuilder strId = new StringBuilder(0);
            Int32 iRequiredSize = 0;
            Int32 iSize = 0;
            Int32 iRet = Win32DeviceMgmt.SetupDiGetDeviceInstanceId(DeviceInfoSet, ref DeviceInfoData, strId, iSize, ref iRequiredSize);
            strId = new StringBuilder(iRequiredSize);
            iSize = iRequiredSize;
            iRet = Win32DeviceMgmt.SetupDiGetDeviceInstanceId(DeviceInfoSet, ref DeviceInfoData, strId, iSize, ref iRequiredSize);
            if (iRet == 1)
            {
                return strId.ToString();
            }

            return String.Empty;
        }

        static String GetDevicePropertyString(IntPtr DeviceInfoSet, Win32DeviceMgmt.SP_DEVINFO_DATA DeviceInfoData, SetupDiGetDeviceRegistryPropertyEnum property)
        {
            byte[] ptrBuf = GetDeviceProperty(DeviceInfoSet, DeviceInfoData, property);
            return ptrBuf.ToStrAuto();
        }

        static Guid GetDevicePropertyGuid(IntPtr DeviceInfoSet, Win32DeviceMgmt.SP_DEVINFO_DATA DeviceInfoData, SetupDiGetDeviceRegistryPropertyEnum property)
        {
            byte[] ptrBuf = GetDeviceProperty(DeviceInfoSet, DeviceInfoData, property);
            return new Guid(ptrBuf);
        }

        static byte[] GetDeviceProperty(IntPtr DeviceInfoSet, Win32DeviceMgmt.SP_DEVINFO_DATA DeviceInfoData, SetupDiGetDeviceRegistryPropertyEnum property)
        {
            StringBuilder strId = new StringBuilder(0);
            byte[] ptrBuf = null;
            UInt32 RegType;
            UInt32 iRequiredSize = 0;
            UInt32 iSize = 0;
            bool iRet = Win32DeviceMgmt.SetupDiGetDeviceRegistryProperty(DeviceInfoSet, ref DeviceInfoData,
                (uint)property, out RegType, ptrBuf, iSize, out iRequiredSize);
            ptrBuf = new byte[iRequiredSize];
            iSize = iRequiredSize;
            iRet = Win32DeviceMgmt.SetupDiGetDeviceRegistryProperty(DeviceInfoSet, ref DeviceInfoData,
                (uint)property, out RegType, ptrBuf, iSize, out iRequiredSize);
            if (iRet)
            {
                return ptrBuf;
            }

            return new byte[0];
        }

    }

    public static class ByteArrayEx
    {
        public static string ToStrAuto(this byte[] bytes)
        {
            string ret = "";


            IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
                // Call unmanaged code
                ret = Marshal.PtrToStringAuto(unmanagedPointer);
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedPointer);
            }
            return ret;
        }
    }
    public class Win32DeviceMgmt
    {
        internal static Int32 ERROR_NO_MORE_FILES = 259;

        internal static Int32 LINE_LEN = 256;

        internal static Int32 DIGCF_DEFAULT = 0x00000001;  // only valid with DIGCF_DEVICEINTERFACE
        internal static Int32 DIGCF_PRESENT = 0x00000002;
        internal static Int32 DIGCF_ALLCLASSES = 0x00000004;
        internal static Int32 DIGCF_PROFILE = 0x00000008;
        internal static Int32 DIGCF_DEVICEINTERFACE = 0x00000010;

        internal static Int32 SPINT_ACTIVE = 0x00000001;
        internal static Int32 SPINT_DEFAULT = 0x00000002;
        internal static Int32 SPINT_REMOVED = 0x00000004;

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            /// <summary>
            /// Size of structure in bytes
            /// </summary>
            public Int32 cbSize;
            /// <summary>
            /// GUID of the device interface class
            /// </summary>
            public Guid ClassGuid;
            /// <summary>
            /// Handle to this device instance
            /// </summary>
            public Int32 DevInst;
            /// <summary>
            /// Reserved; do not use. 
            /// </summary>
            public UIntPtr Reserved;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            /// <summary>
            /// Size of the structure, in bytes
            /// </summary>
            public Int32 cbSize;
            /// <summary>
            /// GUID of the device interface class
            /// </summary>
            public Guid InterfaceClassGuid;
            /// <summary>
            /// 
            /// </summary>
            public Int32 Flags;
            /// <summary>
            /// Reserved; do not use.
            /// </summary>
            public IntPtr Reserved;

        };


        [DllImport("setupapi.dll")]
        internal static extern IntPtr SetupDiGetClassDevsEx(ref Guid ClassGuid, [MarshalAs(UnmanagedType.LPStr)]String enumerator, IntPtr hwndParent, Int32 Flags, IntPtr DeviceInfoSet, [MarshalAs(UnmanagedType.LPStr)]String MachineName, IntPtr Reserved);
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(           // 1st form using a ClassGUID only, with null Enumerator
           ref Guid ClassGuid,
           IntPtr Enumerator,
           IntPtr hwndParent,
           int Flags
        );
        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, IntPtr InterfaceClassGuid, Int32 MemberIndex, ref SP_DEVINFO_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, Int32 MemberIndex, ref SP_DEVINFO_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiClassNameFromGuid(ref Guid ClassGuid, StringBuilder className, Int32 ClassNameSize, ref Int32 RequiredSize);

        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiGetClassDescription(ref Guid ClassGuid, StringBuilder classDescription, Int32 ClassDescriptionSize, ref Int32 RequiredSize);

        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiGetDeviceInstanceId(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            StringBuilder DeviceInstanceId,
            Int32 DeviceInstanceIdSize,
            ref Int32 RequiredSize);

        /// <summary>
        /// The SetupDiGetDeviceRegistryProperty function retrieves the specified device property.
        /// This handle is typically returned by the SetupDiGetClassDevs or SetupDiGetClassDevsEx function.
        /// </summary>
        /// <param Name="DeviceInfoSet">Handle to the device information set that contains the interface and its underlying device.</param>
        /// <param Name="DeviceInfoData">Pointer to an SP_DEVINFO_DATA structure that defines the device instance.</param>
        /// <param Name="Property">Device property to be retrieved. SEE MSDN</param>
        /// <param Name="PropertyRegDataType">Pointer to a variable that receives the registry data Type. This parameter can be NULL.</param>
        /// <param Name="PropertyBuffer">Pointer to a buffer that receives the requested device property.</param>
        /// <param Name="PropertyBufferSize">Size of the buffer, in bytes.</param>
        /// <param Name="RequiredSize">Pointer to a variable that receives the required buffer size, in bytes. This parameter can be NULL.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            uint Property,
            out UInt32 PropertyRegDataType,
            byte[] PropertyBuffer,
            uint PropertyBufferSize,
            out UInt32 RequiredSize
            );

        // Device Property
        [StructLayout(LayoutKind.Sequential)]
        internal struct DEVPROPKEY
        {
            public Guid fmtid;
            public UInt32 pid;
        }

        [DllImport("kernel32.dll")]
        internal static extern Int32 GetLastError();
    }
    /// <summary>
    /// Flags for SetupDiGetDeviceRegistryProperty().
    /// </summary>
    enum SetupDiGetDeviceRegistryPropertyEnum : uint
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
