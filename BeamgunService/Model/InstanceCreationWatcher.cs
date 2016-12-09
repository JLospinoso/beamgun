using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace BeamgunService.Model
{
    public class CallbackAdder : Attribute
    {
    }

    public class InstanceCreationWatcher : IDisposable
    {
        private readonly IDictionary<string, ManagementEventWatcher> _watchers;
        
        public InstanceCreationWatcher()
        {
            _watchers = new Dictionary<string, ManagementEventWatcher>();
        }
        
        [CallbackAdder]
        public void AddNetworkAdapterWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_NetworkAdapter", callback);
        [CallbackAdder]
        public void AddKeyboardWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_Keyboard", callback);
        [CallbackAdder]
        public void AddCdromWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_CDROMDrive", callback);
        [CallbackAdder]
        public void AddDiskWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_DiskDrive", callback);
        [CallbackAdder]
        public void AddFloppyWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_FloppyDrive", callback);
        [CallbackAdder]
        public void AddPhysicalMediaWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_PhysicalMedia", callback);
        [CallbackAdder]
        public void AddTapeDriveWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_TapeDrive", callback);
        [CallbackAdder]
        public void AddFirewireWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_1394Controller", callback);
        [CallbackAdder]
        public void AddIdeWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_IDEController", callback);
        [CallbackAdder]
        public void AddOnMotherboardWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_OnBoardDevice", callback);
        [CallbackAdder]
        public void AddOnParallelPortWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_ParallelPort", callback);
        [CallbackAdder]
        public void AddPlugNPlayWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_PnPEntity", callback);
        [CallbackAdder]
        public void AddScsiWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_SCSIController", callback);
        [CallbackAdder]
        public void AddSerialWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_SerialPort", callback);
        [CallbackAdder]
        public void AddSoundWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_SoundDevice", callback);
        [CallbackAdder]
        public void AddUsbControllerWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_USBController", callback);
        [CallbackAdder]
        public void AddSlotWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_SystemSlot", callback);
        [CallbackAdder]
        public void AddBatteryWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_Battery", callback);
        [CallbackAdder]
        public void AddPortableBatteryWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_PortableBattery", callback);
        [CallbackAdder]
        public void AddPrinterWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_PortableBattery", callback);
        [CallbackAdder]
        public void AddPrintJobWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_PrintJob", callback);
        [CallbackAdder]
        public void AddModemWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_POTSModem", callback);
        [CallbackAdder]
        public void AddMonitorWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_DesktopMonitor", callback);
        [CallbackAdder]
        public void AddVideoControllerWatcher(Action<ManagementBaseObject> callback) => AddWatcher("Win32_VideoController", callback);

        public void AddUniversalWatcher(Action<ManagementBaseObject> callback)
        {
            foreach (var method in typeof(InstanceCreationWatcher).GetMethods())
            {
                var isAdder = method.GetCustomAttributes(typeof(CallbackAdder), false).Length > 0;
                if (isAdder)
                {
                    method.Invoke(this, new object[] {callback});
                }
            }
        }

        public void Dispose()
        {
            lock (_watchers)
            {
                foreach (var pair in _watchers)
                {
                    var watcher = pair.Value;
                    watcher.Stop();
                    watcher.Dispose();
                }
            }
        }
        
        private bool AddWatcher(string type, Action<ManagementBaseObject> callback)
        {
            try
            {
                lock (_watchers)
                {
                    ManagementEventWatcher watcher;
                    if (!_watchers.TryGetValue(type, out watcher))
                    {
                        var networkQuery = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), $"TargetInstance isa \"{type}\"");
                        watcher = new ManagementEventWatcher(networkQuery);
                        _watchers[type] = watcher;
                    }
                    watcher.EventArrived += (caller, args) =>
                    {
                        using (var obj = (ManagementBaseObject) args.NewEvent["TargetInstance"])
                        {
                            callback(obj);
                        }
                    };
                    watcher.Start();
                }
            }
            catch (ManagementException e)
            {
                Debug.WriteLine($"WMI error registering callback for {type}: {e.Message}");
                return false;
            }
            return true;
        }
    }
}
