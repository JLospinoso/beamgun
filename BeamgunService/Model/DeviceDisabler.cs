using System;
using System.Diagnostics;
using System.Management;

namespace BeamgunService.Model
{
    public class DeviceDisablerException : Exception
    {
        public DeviceDisablerException(string s, Exception e) : base(s, e) { }
    }

    public class DeviceDisabler
    {
        public bool DisableNoArgs(ManagementBaseObject managementBaseObject) => Disable(managementBaseObject, "Disable", null);
        public bool DisableTrue(ManagementBaseObject managementBaseObject) => Disable(managementBaseObject, "Disable", new object[] { true });
        public bool PowerOff(ManagementBaseObject managementBaseObject) => Disable(managementBaseObject, "SetPowerState", new object[] { 6, 0 });

        private bool Disable(ManagementBaseObject x, string methodName, object[] arguments)
        {
            var query = $"SELECT * FROM {x.ClassPath.ClassName}";
            var selectedDeviceId = x["DeviceId"].ToString();
            try
            {
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (var item in searcher.Get())
                    {
                        if (item["DeviceId"].ToString() != selectedDeviceId)
                        {
                            continue;
                        };
                        var managementObject = (ManagementObject)item;
                        managementObject.InvokeMethod(methodName, arguments);
                        return true;
                    }
                }
            }
            catch (ManagementException e)
            {
                Debug.WriteLine($"Error disabling {x.Properties["Caption"].Value}: {e.Message}");
                return false;
            }
            Debug.WriteLine($"Could not find {selectedDeviceId}");
            return false;
        }

    }
}
