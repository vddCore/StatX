using System;
using System.Diagnostics;
using OverCR.StatX.Hooks.WinAPI;

namespace OverCR.StatX.Hooks.Keyboard
{
    public class KeyboardHook : InputHook
    {
        private const int KeyboardHookID = 13;

        public delegate void KeyboardHookEventHandler(KeyboardHookEventArgs e);

        public event KeyboardHookEventHandler KeyDown;
        public event KeyboardHookEventHandler KeyUp;

        public override void Install()
        {
            HookHandler = HookMethod;
            HookID = SetKeyboardHandler(HookHandler);
        }

        private IntPtr HookMethod(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                var eventArgs = new KeyboardHookEventArgs(wParam, lParam);

                switch ((Messages.Keyboard)wParam)
                {
                    case Messages.Keyboard.SystemKeyDown:
                    case Messages.Keyboard.KeyDown:
                        KeyDown?.Invoke(eventArgs);
                        break;
                    case Messages.Keyboard.SystemKeyUp:
                    case Messages.Keyboard.KeyUp:
                        KeyUp?.Invoke(eventArgs);
                        break;
                }
            }
            return User32.CallNextHookEx(HookID, code, wParam, lParam);
        }

        private IntPtr SetKeyboardHandler(User32.InputHookHandler hook)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
            {
                return User32.SetWindowsHookEx(KeyboardHookID, hook, Kernel32.GetModuleHandle(module.ModuleName), 0);
            }
        }
    }
}
