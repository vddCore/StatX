using System;
using System.Diagnostics;
using OverCR.StatX.Hooks.WinAPI;

namespace OverCR.StatX.Hooks
{
    public class MouseHook
    {
        private const int MouseHookID = 14;

        private IntPtr HookID { get; set; }
        private User32.InputHookHandler HookHandler { get; set; }

        public delegate void MouseHookEventHandler(MouseHookEventArgs e);

        public bool Installed => HookID != IntPtr.Zero;

        public event MouseHookEventHandler MouseButtonDown;
        public event MouseHookEventHandler MouseButtonUp;
        public event MouseHookEventHandler LeftMouseButtonDown;
        public event MouseHookEventHandler LeftMouseButtonUp;
        public event MouseHookEventHandler LeftMouseButtonDoubleClick;
        public event MouseHookEventHandler MiddleMouseButtonDown;
        public event MouseHookEventHandler MiddleMouseButtonUp;
        public event MouseHookEventHandler RightMouseButtonDown;
        public event MouseHookEventHandler RightMouseButtonUp;
        public event MouseHookEventHandler MouseMove;
        public event MouseHookEventHandler MouseScroll;

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

        private static IntPtr SetMouseHandler(User32.InputHookHandler hook)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
            {
                return User32.SetWindowsHookEx(MouseHookID, hook, Kernel32.GetModuleHandle(module.ModuleName), 0);
            }
        }
    }
}
