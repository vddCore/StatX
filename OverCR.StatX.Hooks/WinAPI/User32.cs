using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI
{
    class User32
    {
        internal delegate IntPtr InputHookHandler(int code, IntPtr wParam, IntPtr lParam);
        internal delegate void WindowsEventHandler(IntPtr hookId, uint eventId, IntPtr windowHandle, int objectId, int childId, 
            uint eventThread, uint timestamp);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int hookId, InputHookHandler handler, IntPtr moduleHandle, uint threadId);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr hookId);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr hookId, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr moduleHandle, WindowsEventHandler hookHandler, 
            uint processId, uint threadId, uint flags);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr hookId);
    }
}
