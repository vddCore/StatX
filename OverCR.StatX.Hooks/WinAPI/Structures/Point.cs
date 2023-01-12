using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Point
    {
        internal int X;
        internal int Y;
    }
}
