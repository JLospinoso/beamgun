using System;
using System.Management;

namespace BeamgunApp.Models
{
    public class MouseWatcher : IDisposable
    {
        private readonly ManagementEventWatcher _watcher;

        public MouseWatcher(IBeamgunSettings settings, WorkstationLocker locker, Action<string> report, Action<string> alarm, Func<bool> disabled)
        {
            var MouseQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_PointingDevice\"");
            _watcher = new ManagementEventWatcher(MouseQuery);
            _watcher.EventArrived += (caller, args) =>
            {
                if (disabled()) return;
                var obj = (ManagementBaseObject)args.NewEvent["TargetInstance"];
                alarm($"Alerting on mouse insertion: " +
                                   $"{obj["Name"]} " +
                                   $"{obj["Caption"]} " +
                                   $"{obj["Description"]} " +
                                   $"{obj["DeviceID"]}" +
                                   $"{obj["Manufacturer"]} " +
                                   $"{obj["PNPDeviceID"]}.");
                if (!settings.LockOnMouse) return;
                if (WhiteList.WhiteListed(obj))
                {
                    report($"Device is whitelisted, remove {obj["DeviceID"]} from {WhiteList.WhiteFilename} if you've changed your mind.");
                    return;
                }
                report(locker.Lock()
                    ? "Successfully locked the workstation."
                    : "Could not lock the workstation.");

            };
            _watcher.Start();
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}
