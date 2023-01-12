using System;
using System.Text;
using OverCR.StatX.Hooks.WinAPI;
using OverCR.StatX.Hooks.WinAPI.Structures;

namespace OverCR.StatX.Hooks.Windows
{
    public class WindowHookEventArgs : EventArgs
    {
        public string Title { get; private set; }
        public IntPtr Handle { get; }
        public IntPtr ProcessHandle { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public WindowHookEventArgs(IntPtr windowHandle)
        {
            Handle = windowHandle;

            DetermineWindowName();
            DetermineWindowDimensions();
            DetermineParentProcess();
        }

        private void DetermineWindowName()
        {
            var sb = new StringBuilder(User32.GetWindowTextLength(Handle) + 1);
            User32.GetWindowText(Handle, sb, sb.Capacity);

            Title = sb.ToString();
        }

        private void DetermineWindowDimensions()
        {
            Rectangle rect;
            User32.GetWindowRect(Handle, out rect);

            Height = rect.Bottom - rect.Top;
            Width = rect.Right - rect.Left;

            DetermineWindowCoordinates(rect);
        }

        private void DetermineWindowCoordinates(Rectangle rect)
        {
            X = rect.Left;
            Y = rect.Top;
        }

        private void DetermineParentProcess()
        {
            ProcessHandle = OleAcc.GetProcessHandleFromHwnd(Handle);
        }
    }
}
