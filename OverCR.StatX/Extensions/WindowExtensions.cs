using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace OverCR.StatX.Extensions
{
    public static class WindowExtensions
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr handle, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr handle, int index, int newLong);

        public static void DisableMaximization(this Window window)
        {
            int style = -16;
            int maximizeBox = 0x10000;

            var handle = new WindowInteropHelper(window).Handle;
            var currentWindowLong = GetWindowLong(handle, style);
            SetWindowLong(handle, style, currentWindowLong & ~maximizeBox);
        }
    }
}
