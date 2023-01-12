using System;
using System.Diagnostics;
using OverCR.StatX.Hooks.WinAPI;

namespace OverCR.StatX.Hooks
{
    public class MouseHook
    {
        private const int MouseHookID = 14;

        private IntPtr HookID { get; set; }
        private User32.MouseHookHandler HookHandler { get; set; }

        public delegate void MouseHookCallback(MouseHookEventArgs e);

        public bool Installed => HookID != IntPtr.Zero;

        public event MouseHookCallback MouseButtonDown;
        public event MouseHookCallback MouseButtonUp;
        public event MouseHookCallback LeftMouseButtonDown;
        public event MouseHookCallback LeftMouseButtonUp;
        public event MouseHookCallback LeftMouseButtonDoubleClick;
        public event MouseHookCallback MiddleMouseButtonDown;
        public event MouseHookCallback MiddleMouseButtonUp;
        public event MouseHookCallback RightMouseButtonDown;
        public event MouseHookCallback RightMouseButtonUp;
        public event MouseHookCallback MouseMove;
        public event MouseHookCallback MouseScroll;

        ~MouseHook()
        {
            Uninstall();
        }

        public void Install()
        {
            HookHandler = HookMethod;
            HookID = SetMouseHandler(HookHandler);
        }

        public void Uninstall()
        {
            if (Installed)
            {
                User32.UnhookWindowsHookEx(HookID);
                HookID = IntPtr.Zero;
            }
        }

        private IntPtr HookMethod(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                var eventArgs = new MouseHookEventArgs(wParam, lParam);

                switch ((Messages.Mouse)wParam)
                {
                    case Messages.Mouse.LeftButtonDown:
                        MouseButtonDown?.Invoke(eventArgs);
                        LeftMouseButtonDown?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.LeftButtonUp:
                        MouseButtonUp?.Invoke(eventArgs);
                        LeftMouseButtonUp?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.MiddleButtonDown:
                        MouseButtonDown?.Invoke(eventArgs);
                        MiddleMouseButtonDown?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.MiddleButtonUp:
                        MouseButtonUp?.Invoke(eventArgs);
                        MiddleMouseButtonUp?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.RightButtonDown:
                        MouseButtonDown?.Invoke(eventArgs);
                        RightMouseButtonDown?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.RightButtonUp:
                        MouseButtonUp?.Invoke(eventArgs);
                        RightMouseButtonUp?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.LeftButtonDoubleClick:
                        LeftMouseButtonDoubleClick?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.MouseWheel:
                        MouseScroll?.Invoke(eventArgs);
                        break;
                    case Messages.Mouse.MouseMove:
                        MouseMove?.Invoke(eventArgs);
                        break;
                }
            }
            return User32.CallNextHookEx(HookID, code, wParam, lParam);
        }

        private IntPtr SetMouseHandler(User32.MouseHookHandler hook)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
            {
                return User32.SetWindowsHookEx(MouseHookID, hook, Kernel32.GetModuleHandle(module.ModuleName), 0);
            }
        }
    }
}
