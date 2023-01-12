using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using OverCR.StatX.Hooks.Windows;
using OverCR.StatX.Statistics;

namespace OverCR.StatX
{
    public partial class App
    {
        private NotifyIcon NotifyIcon { get; }

        public static event EventHandler NotifyIconClicked;

        public App()
        {
            var stream = GetResourceStream(new Uri("pack://application:,,,/OverCR.StatX;component/Resources/Icons/TrayIcon.ico"))?.Stream;

            if (stream == null)
                throw new Exception("Icon stream cannot be null.");

            NotifyIcon = new NotifyIcon
            {
                Icon = new Icon(stream),
                Visible = true,
                Text = "StatX"
            };

            NotifyIcon.Click += (sender, args) =>
            {
                NotifyIconClicked?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}
