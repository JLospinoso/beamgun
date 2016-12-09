using System;
using System.Management;

namespace BeamgunService.Model
{
    public class NetworkAdapterDisablerException : Exception {
        public NetworkAdapterDisablerException(string s, Exception e) : base(s, e) {  }
    }
    public class NetworkAdapterDisabler
    {
        public bool Disable(string deviceId)
        {
            var query = $"SELECT * FROM Win32_NetworkAdapter WHERE DeviceID = \"{deviceId}\"";
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (var item in searcher.Get())
                {
                    var managementObject = (ManagementObject)item;
                    try
                    {
                        managementObject.InvokeMethod("Disable", null);
                        return true;
                    }
                    catch (ManagementException e)
                    {
                        throw new NetworkAdapterDisablerException("Error disabling new network adapter.", e);
                    }
                }
            }
            return false;
        }
    }
}
