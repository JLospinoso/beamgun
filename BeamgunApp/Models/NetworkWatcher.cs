using System;
using System.Management;

namespace BeamgunApp.Models
{
    public class NetworkWatcher : IDisposable
    {
        public delegate void Report(string message);
        public delegate void TripAlarm(string message);

        private readonly ManagementEventWatcher _watcher;

        public NetworkWatcher(IBeamgunSettings settings, Report report, TripAlarm alarm)
        {
            var networkQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_NetworkAdapter\"");
            _watcher = new ManagementEventWatcher(networkQuery);
            _watcher.EventArrived += (caller, args) =>
            {
                var obj = (ManagementBaseObject)args.NewEvent["TargetInstance"];
                var alertMessage = $"Alerting on network adapter insertion: " +
                                   $"{obj["AdapterType"]} " +
                                   $"{obj["Caption"]} " +
                                   $"{obj["Description"]} " +
                                   $"{obj["DeviceID"]} " +
                                   $"{obj["GUID"]} " +
                                   $"{obj["MACAddress"]} " +
                                   $"{obj["Manufacturer"]} " +
                                   $"{obj["Name"]} " +
                                   $"{obj["PermanentAddress"]} " +
                                   $"{obj["NetworkAddresses"]} " +
                                   $"{obj["ProductName"]} " +
                                   $"{obj["ServiceName"]} " +
                                   $"{obj["SystemCreationClassName"]} " +
                                   $"{obj["SystemName"]} ";
                alarm(alertMessage);
                if (!settings.DisableNetworkAdapter) return;
                // TODO: Parse out into separate class?
                var query = $"SELECT * FROM Win32_NetworkAdapter WHERE DeviceID = \"{obj["DeviceID"]}\"";
                var searcher = new ManagementObjectSearcher(query);
                foreach (var item in searcher.Get())
                {
                    var managementObject = (ManagementObject)item;
                    try
                    {
                        var disableCode = (uint)managementObject.InvokeMethod("Disable", null);
                        report(disableCode == 0
                            ? "Network adapter successfully disabled."
                            : $"Danger! Unable to disable network adapter: {disableCode}");
                        return;
                    }
                    catch (ManagementException e)
                    {
                        report($"Error disabling new network adapter: {e}");
                    }
                }
            };
            _watcher.Start();
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}
