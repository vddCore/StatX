using System;
using System.Drawing;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace OverCR.StatX.Tray
{
    public class TrayIconProvider
    {
        private NotifyIcon NotifyIcon { get; set; }

        public event EventHandler NotifyIconClicked;

        public void CreateTrayIcon(string text)
        {
            var stream = Application.GetResourceStream(new Uri("pack://application:,,,/OverCR.StatX;component/Resources/Icons/TrayIcon.ico"))?.Stream;

            if (stream == null)
                App.CriticalFailure("Couldn't get tray icon stream. Your StatX executable is probably corrupted.", Application.Current.MainWindow);

            NotifyIcon = new NotifyIcon
            {
                Icon = new Icon(stream),
                Visible = true,
                Text = text
            };
            NotifyIcon.Click += (s, a) => NotifyIconClicked?.Invoke(this, EventArgs.Empty);
        }

        public void HideIcon()
        {
            NotifyIcon.Visible = false;
        }
    }
}
