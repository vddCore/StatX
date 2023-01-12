using System;
using System.Runtime.InteropServices;
using OverCR.StatX.Hooks.WinAPI;
using OverCR.StatX.Hooks.WinAPI.Structures;

namespace OverCR.StatX.Hooks
{
    public class MouseHookEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }

        public MouseButton Button { get; private set; }
        public bool Pressed { get; private set; }
        public bool DoubleClick { get; private set; }

        public bool WasScrolled { get; }
        public bool BackwardsScroll { get; }
        public int WheelDelta { get; }
        public int TimesScrolled { get; }

        public bool EventInjected { get; }
        public bool EventInjectedLowIntegrity { get; }

        public uint Timestamp { get; }
        public int ExtraInfo { get; }

        public MouseHookEventArgs(IntPtr wParam, IntPtr lParam)
        {
            var structure = Marshal.PtrToStructure<MouseLowLevelHookStruct>(lParam);

            X = structure.Point.X;
            Y = structure.Point.Y;
            Button = MouseButton.None;

            EventInjected = structure.Flags == 1;
            EventInjectedLowIntegrity = structure.Flags == 2;

            Timestamp = structure.Time;
            ExtraInfo = structure.ExtraInfo.ToInt32();

            if ((Messages.Mouse)wParam == Messages.Mouse.MouseWheel)
            {
                WasScrolled = true;
                BackwardsScroll = WheelDelta < 0;

                WheelDelta = (int)structure.MouseData >> 16;
                TimesScrolled = Math.Abs(WheelDelta / 120);
            }
            DetermineMouseArgs((Messages.Mouse)wParam);
        }

        private void DetermineMouseArgs(Messages.Mouse mouseMessage)
        {
            if (mouseMessage == Messages.Mouse.LeftButtonDown)
            {
                Button = MouseButton.Left;
                Pressed = true;
            }

            if (mouseMessage == Messages.Mouse.LeftButtonUp)
            {
                Button = MouseButton.Left;
                Pressed = false;
            }

            if (mouseMessage == Messages.Mouse.MiddleButtonDown)
            {
                Button = MouseButton.Middle;
                Pressed = true;
            }

            if (mouseMessage == Messages.Mouse.MiddleButtonUp)
            {
                Button = MouseButton.Middle;
                Pressed = false;
            }

            if (mouseMessage == Messages.Mouse.RightButtonDown)
            {
                Button = MouseButton.Right;
                Pressed = true;
            }

            if (mouseMessage == Messages.Mouse.RightButtonUp)
            {
                Button = MouseButton.Right;
                Pressed = false;
            }

            if (mouseMessage == Messages.Mouse.LeftButtonDoubleClick)
            {
                Button = MouseButton.Left;
                Pressed = true;
                DoubleClick = true;
            }
        }
    }
}
