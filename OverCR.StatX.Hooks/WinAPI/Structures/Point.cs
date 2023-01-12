using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;
    }
}
