using System;

namespace OverCR.StatX.Hooks.WinAPI
{
    [Flags]
    internal enum WindowsEventFlags
    {
        OutOfContext = 0,
        SkipOwnThread = 1,
        SkipOwnProcess = 2,
        InContext = 4
    }
}
