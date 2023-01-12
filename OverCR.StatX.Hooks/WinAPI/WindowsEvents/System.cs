namespace OverCR.StatX.Hooks.WinAPI.WindowsEvents
{
    internal enum System
    {
        SoundPlayed = 0x0001,
        Alert = 0x0002,
        ForegroundWindowChanged = 0x0003,
        ApplicationBarMenuOpened = 0x0004,
        ApplicationBarMenuClosed = 0x0005,
        PopupMenuDisplayed = 0x0006,
        PopupMenuClosed = 0x0007,
        WindowCapturedMouse = 0x0008,
        WindowLostMouse = 0x0009,
        ScrollingStarted = 0x0012,
        ScrollingEnded = 0x0013,
        AltTabPressed = 0x0014,
        AltTabReleased = 0x0015,
        WindowMoveOrResizeStarted = 0x000A,
        WindowMoveOrResizeFinished = 0x000B,
        ContextHelpOpened = 0x000C,
        ContextHelpClosed = 0x000D,
        DragDropModeEntered = 0x000E,
        DragDropModeExited = 0x000F,
        DialogBoxOpened = 0x0010,
        DialogBoxClosed = 0x0011,
        WindowMinimized = 0x0016,
        WindowRestored = 0x0017,
        ActiveDesktopSwitched = 0x0020,
        End = 0x00FF
    }
}
