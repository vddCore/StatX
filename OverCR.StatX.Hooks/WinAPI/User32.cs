using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI
{
    public class User32
    {
        public delegate IntPtr InputHookHandler(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int hookId, InputHookHandler handler, IntPtr handle, uint threadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);
    }
}
