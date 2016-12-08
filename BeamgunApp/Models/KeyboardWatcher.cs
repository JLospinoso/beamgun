using System;
using System.Management;

namespace BeamgunApp.Models
{
    public class KeyboardWatcher : IDisposable
    {
        private readonly ManagementEventWatcher _watcher;

        public KeyboardWatcher(IBeamgunSettings settings, WorkstationLocker locker, Action<string> report, Action<string> alarm)
        {
            var keyboardQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Keyboard\"");
            _watcher = new ManagementEventWatcher(keyboardQuery);
            _watcher.EventArrived += (caller, args) =>
            {
                using (var obj = (ManagementBaseObject) args.NewEvent["TargetInstance"])
                {
                   alarm("Alerting on keyboard insertion: " +
                   $"{obj["Name"]} " +
                   $"{obj["Caption"]} " +
                   $"{obj["Description"]} " +
                   $"{obj["DeviceID"]} " +
                   $"{obj["Layout"]} " +
                   $"{obj["PNPDeviceID"]}.");
                    if (!settings.LockWorkstation) return;
                    report(locker.Lock()
                        ? "Successfully locked the workstation."
                        : "Could not lock the workstation.");
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
