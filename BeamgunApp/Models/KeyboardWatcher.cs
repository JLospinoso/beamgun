using System;
using System.Management;

namespace BeamgunApp.Models
{
    public class KeyboardWatcher : IDisposable
    {
        private readonly ManagementEventWatcher _watcher;

        public KeyboardWatcher(IBeamgunSettings settings, WorkstationLocker locker, Action<string> report, Action<string> alarm, Func<bool> disabled)
        {
            var keyboardQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Keyboard\"");
            _watcher = new ManagementEventWatcher(keyboardQuery);
            _watcher.EventArrived += (caller, args) =>
            {
                if (disabled()) return;
                var obj = (ManagementBaseObject)args.NewEvent["TargetInstance"];
                alarm($"Alerting on keyboard insertion: " +
                                   $"{obj["Name"]} " +
                                   $"{obj["Caption"]} " +
                                   $"{obj["Description"]} " +
                                   $"DeviceID[{obj["DeviceID"]}]" +
                                   $"{obj["Layout"]} " +
                                   $"{obj["PNPDeviceID"]}.");
                if (!settings.LockOnKeyboard) return;
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
