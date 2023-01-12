using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    struct MouseLowLevelHookStruct
    {
        public Point Point;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }
}
