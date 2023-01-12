using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI
{
    class Kernel32
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetModuleHandle(string moduleName);
    }
}
