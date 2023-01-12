using System;
using System.Diagnostics;
using OverCR.StatX.Hooks.WinAPI;

namespace OverCR.StatX.Hooks.Windows
{
    public class SystemEventHook
    {
        private IntPtr HookID { get; set; }
        private User32.WindowsEventHandler HookHandler { get; set; }

        public bool Installed => HookID != IntPtr.Zero;

        public delegate void WindowHookEventHandler(WindowHookEventArgs e);

        public event WindowHookEventHandler ActiveWindowChanged;
        public event WindowHookEventHandler WindowMinimized;
        public event WindowHookEventHandler WindowRestored;
        public event WindowHookEventHandler MouseCaptured;
        public event WindowHookEventHandler MouseReleased;

        ~SystemEventHook()
        {
            Uninstall();
        }

        public void Install()
        {
            HookHandler = HookMethod;
            HookID = SetHookHandler(HookHandler);
        }

        public void Uninstall()
        {
            if (Installed)
            {
                User32.UnhookWinEvent(HookID);
            }
        }

        public void HookMethod(IntPtr hookId, uint eventId, IntPtr windowHandle, int objectId, int childId,
            uint eventThread, uint timestamp)
        {
            switch ((WinAPI.WindowsEvents.System)eventId)
            {
                case WinAPI.WindowsEvents.System.ForegroundWindowChanged:
                    var awcArgs = new WindowHookEventArgs(windowHandle);
                    ActiveWindowChanged?.Invoke(awcArgs);
                    break;
                case WinAPI.WindowsEvents.System.WindowMinimized:
                    var wmArgs = new WindowHookEventArgs(windowHandle);
                    WindowMinimized?.Invoke(wmArgs);
                    break;
                case WinAPI.WindowsEvents.System.WindowRestored:
                    var wrArgs = new WindowHookEventArgs(windowHandle);
                    WindowRestored?.Invoke(wrArgs);
                    break;
                case WinAPI.WindowsEvents.System.WindowCapturedMouse:
                    var mcArgs = new WindowHookEventArgs(windowHandle);
                    MouseCaptured?.Invoke(mcArgs);
                    break;
                case WinAPI.WindowsEvents.System.WindowLostMouse:
                    var mrArgs = new WindowHookEventArgs(windowHandle);
                    MouseReleased?.Invoke(mrArgs);
                    break;
            }
        }

        private static IntPtr SetHookHandler(User32.WindowsEventHandler hook)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
            {
                return User32.SetWinEventHook(
                    (uint)WinAPI.WindowsEvents.System.Alert,
                    (uint)WinAPI.WindowsEvents.System.End,
                    Kernel32.GetModuleHandle(module.ModuleName),
                    hook, 0, 0,
                    WindowsEventFlags.OutOfContext | WindowsEventFlags.SkipOwnThread
                );
            }
        }

    }
}
