using System;
using OverCR.StatX.Hooks.WinAPI;

namespace OverCR.StatX.Hooks
{
    public abstract class InputHook
    {
        internal IntPtr HookID { get; set; }
        internal User32.InputHookHandler HookHandler { get; set; }

        public bool Installed => HookID != IntPtr.Zero;

        ~InputHook()
        {
            Uninstall();
        }

        public virtual void Install()
        {
            throw new NotImplementedException("THis method must be overriden.");
        }

        public void Uninstall()
        {
            if (Installed)
            {
                User32.UnhookWindowsHookEx(HookID);
            }
        }
    }
}
