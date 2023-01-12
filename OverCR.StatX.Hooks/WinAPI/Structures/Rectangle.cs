using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Rectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
