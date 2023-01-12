using System;
using System.Runtime.InteropServices;
using OverCR.StatX.Hooks.WinAPI;

namespace OverCR.StatX.Hooks
{
    public class KeyboardHookEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool Pressed { get; private set; }

        public KeyboardHookEventArgs(IntPtr wParam, IntPtr lParam)
        {
            Key = (Keys)Marshal.ReadInt32(lParam);
            DeterminePressed(wParam);
        }

        private void DeterminePressed(IntPtr wParam)
        {
            switch ((Messages.Keyboard)wParam)
            {
                case Messages.Keyboard.SystemKeyDown:
                case Messages.Keyboard.KeyDown:
                    Pressed = true;
                    break;
                case Messages.Keyboard.SystemKeyUp:
                case Messages.Keyboard.KeyUp:
                    Pressed = false;
                    break;
            }
        }
    }
}
