using System;
using System.Management;
using System.Threading;

namespace BeamgunApp.Models
{
    public class NetworkWatcher : IDisposable
    {
        public bool Triggered { get; set; }
        private readonly ManagementEventWatcher _watcher;
        
        public NetworkWatcher(IBeamgunSettings settings, NetworkAdapterDisabler networkAdapterDisabler, 
            Action<string> report, Action<string> alarm)
        {
            var networkQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_NetworkAdapter\"");
            _watcher = new ManagementEventWatcher(networkQuery);
            _watcher.EventArrived += (caller, args) =>
            {
                using (var obj = (ManagementBaseObject) args.NewEvent["TargetInstance"])
                {
                    var alertMessage = $"Alerting on network adapter insertion: {obj["Description"]} (Device ID {obj["DeviceID"]}) ";
                    alarm(alertMessage);
                    Triggered = settings.DisableNetworkAdapter;
                    if (Triggered)
                    {
                        report($"Disabling {obj["Description"]} every {settings.DisableNetworkAdapterInterval} ms until Reset.");
                    }
                    while (Triggered)
                    {
                        try
                        {
                            if (!networkAdapterDisabler.Disable(obj["DeviceID"].ToString()))
                            {
                                report($"DANGER: Unable to disable {obj["AdapterType"]}!");
                            }
                            Thread.Sleep((int)settings.DisableNetworkAdapterInterval);
                        }
                        catch (NetworkAdapterDisablerException e)
                        {
                            report(e.Message);
                        }
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
