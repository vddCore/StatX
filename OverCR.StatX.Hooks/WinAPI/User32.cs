using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI
{
    class User32
    {
        internal delegate IntPtr InputHookHandler(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int hookId, InputHookHandler handler, IntPtr handle, uint threadId);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);
    }
}
