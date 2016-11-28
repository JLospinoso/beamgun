using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace BeamgunApp.Models
{
    public class KeystrokeHooker : IDisposable
    {
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private event LowLevelKeyboardProc KeyboardHook;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate void Keypress(Keys key);
        public event Keypress Callback;
        private const int KeyboardConst = 13;
        private const int KeydownConst = 0x0100;
        private readonly IntPtr _hookId;
        private static int _hookSet = 0;

        public KeystrokeHooker()
        {
            var hookNotSet = Interlocked.Exchange(ref _hookSet, 1) == 0;
            if (hookNotSet)
            {
                KeyboardHook = (nCode, wParam, lParam) =>
                {
                    if (nCode >= 0 && wParam == (IntPtr)KeydownConst)
                    {
                        var keyCode = Marshal.ReadInt32(lParam);
                        var key = (Keys)keyCode;
                        Callback?.Invoke(key);
                    }
                    return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
                };
                _hookId = SetHook();
            }
            else
            {
                throw new Exception("Hook has already been set.");
            }
        }

        private IntPtr SetHook()
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(KeyboardConst, KeyboardHook, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        
        public void Dispose()
        {
            GC.KeepAlive(KeyboardHook);
            UnhookWindowsHookEx(_hookId);
            Interlocked.Exchange(ref _hookSet, 0);
        }
    }
}
