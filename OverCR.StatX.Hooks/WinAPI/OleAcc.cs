using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI
{
    public class OleAcc
    {
        [DllImport("OleAcc.dll")]
        internal static extern IntPtr GetProcessHandleFromHwnd(IntPtr windowHandle);
    }
}
