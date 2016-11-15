using System.Runtime.InteropServices;

namespace BeamgunApp.Models
{
    public class WorkstationLocker
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWorkStation();

        public bool Lock()
        {
            return LockWorkStation();
        }
    }
}
