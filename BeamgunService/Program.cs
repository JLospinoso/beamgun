using System;
using System.Management;
using System.Threading;
using BeamgunService.Model;

namespace BeamgunService
{
    public class Program
    {
        private static void ObserveIt(ManagementBaseObject x)
        {
            var deviceId = x["DeviceId"].ToString();
            var className = x.ClassPath.ClassName;
            Console.WriteLine($"[ ] Inserted a {className} {x["Name"]} {x["Caption"]} {deviceId}");
        }
        
        private static void DisableDevice(ManagementBaseObject x)
        {
            DisableHardware.DisableDevice(deviceString =>
            {
                var deviceId = x["DeviceID"].ToString();
                var hwId = deviceString[DisableHardware.SetupDiGetDeviceRegistryPropertyEnum.SPDRP_HARDWAREID];
                if (hwId.StartsWith(deviceId.Substring(0, 6)))
                {
                    Console.WriteLine("*************");
                    Console.WriteLine(deviceId);
                    Console.WriteLine(deviceString);
                    Console.WriteLine("*************");
                }
                else
                {
                    Console.WriteLine(hwId);
                }

                return false;
            });
        }

        public static void Main(string[] args)
        {
            var disabler = new DeviceDisabler();
            var watcher = new InstanceCreationWatcher();

            watcher.AddUniversalWatcher(ObserveIt);
            /*
            watcher.AddPlugNPlayWatcher(x =>
            {
                Console.WriteLine($"[ ] Disabling PlugNPlay Device {x.ClassPath}");
                while (disabler.DisableTrue(x)) { Console.Write("."); }
            });
            watcher.AddNetworkAdapterWatcher(x =>
            {
                Console.WriteLine($"[ ] Disabling NetworkAdapter Device {x.ClassPath}");
                while (disabler.DisableNoArgs(x)) { Console.Write("."); }
            });
            */
            watcher.AddKeyboardWatcher(DisableDevice);

            Thread.Sleep(int.MaxValue);
        }
    }
}
