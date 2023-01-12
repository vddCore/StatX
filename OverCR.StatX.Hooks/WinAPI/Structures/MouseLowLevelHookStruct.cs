using System;
using System.Runtime.InteropServices;

namespace OverCR.StatX.Hooks.WinAPI.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseLowLevelHookStruct
    {
        internal Point Point;
        internal uint MouseData;
        internal uint Flags;
        internal uint Time;
        internal IntPtr ExtraInfo;
    }
}
