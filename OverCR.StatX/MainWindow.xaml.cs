using System;
using System.Globalization;
using System.Windows;
using OverCR.StatX.Statistics;

namespace OverCR.StatX
{
    public partial class MainWindow
    {
        private MouseTracker MouseTracker { get; set; }
        private KeyboardTracker KeyboardTracker { get; set; }
        private NetworkTracker NetworkTracker { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void KeyboardTracker_KeyPressesChanged(object sender, EventArgs e)
        {
            TotalKeyPressesDisplay.Value = KeyboardTracker.TotalKeyPresses;
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalKeyPresses", KeyboardTracker.TotalKeyPresses.ToString());
        }

        private void KeyboardTracker_KeyPressureChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalKeyPressEnergy", KeyboardTracker.TotalKeypressEnergy.ToString(CultureInfo.InvariantCulture));
        }

        private void MouseTracker_DistanceTraveledChanged(object sender, EventArgs e)
        {
            TotalMouseDistanceDisplay.Value = Math.Round(MouseTracker.TotalDistanceTraveled, 2);
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalMouseTravelDistance", MouseTracker.TotalDistanceTraveled.ToString(CultureInfo.InvariantCulture));
        }

        private void MouseTracker_DistanceScrolledChanged(object sender, EventArgs e)
        {
            TotalMouseScrollDistanceDisplay.Value = Math.Round(MouseTracker.TotalDistanceScrolled, 2);
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalMouseScrollDistance", MouseTracker.TotalDistanceScrolled.ToString(CultureInfo.InvariantCulture));
        }

        private void MouseTracker_MiddleClicksChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalMouseMiddleClicks", MouseTracker.TotalMiddleClicks.ToString());
        }

        private void MouseTracker_RightClicksChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalMouseRightClicks", MouseTracker.TotalRightClicks.ToString());
        }

        private void MouseTracker_LeftClicksChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalMouseLeftClicks", MouseTracker.TotalLeftClicks.ToString());
        }

        private void MouseTracker_ClicksChanged(object sender, EventArgs e)
        {
            TotalMouseClicksDisplay.Value = MouseTracker.TotalClicks;
        }

        private void NetworkTracker_SentDataChanged(object sender, EventArgs e)
        {
            var megaBytes = Math.Round(NetworkTracker.TotalBytesSent / 1024 / 1024, 2);

            if (megaBytes > 1024)
            {
                var gigaBytes = Math.Round(megaBytes / 1024, 2);
                if (gigaBytes > 1024)
                {
                    var teraBytes = Math.Round(gigaBytes / 1024, 2);
                    SetTotalDataSent(teraBytes, "terabytes");
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataSent", teraBytes.ToString(CultureInfo.InvariantCulture));
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataSentUnit", "terabytes");
                }
                else
                {
                    SetTotalDataSent(gigaBytes, "gigabytes");
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataSent", gigaBytes.ToString(CultureInfo.InvariantCulture));
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataSentUnit", "gigabytes");
                }
            }
            else
            {
                SetTotalDataSent(megaBytes, "megabytes");
                App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataSent", megaBytes.ToString(CultureInfo.InvariantCulture));
                App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataSentUnit", "megabytes");
            }
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalBytesSent", NetworkTracker.TotalBytesSent.ToString(CultureInfo.InvariantCulture));
        }

        private void NetworkTracker_ReceivedDataChanged(object sender, EventArgs e)
        {
            var megaBytes = Math.Round(NetworkTracker.TotalBytesReceived / 1024 / 1024, 2);

            if (megaBytes > 1024)
            {
                var gigaBytes = Math.Round(megaBytes / 1024, 2);
                if (gigaBytes > 1024)
                {
                    var teraBytes = Math.Round(gigaBytes / 1024, 2);
                    SetTotalDataReceived(teraBytes, "terabytes");
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataReceived", teraBytes.ToString(CultureInfo.InvariantCulture));
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataReceivedUnit", "terabytes");
                }
                else
                {
                    SetTotalDataReceived(gigaBytes, "gigabytes");
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataReceived", gigaBytes.ToString(CultureInfo.InvariantCulture));
                    App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataReceivedUnit", "gigabytes");
                }
            }
            else
            {
                SetTotalDataReceived(megaBytes, "megabytes");
                App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataReceived", megaBytes.ToString(CultureInfo.InvariantCulture));
                App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalDataReceivedUnit", "megabytes");
            }
            App.StatisticsSaveFile.Section("Main").SetEntryValue("TotalBytesReceived", NetworkTracker.TotalBytesSent.ToString(CultureInfo.InvariantCulture));
        }

        private void SetTotalDataSent(double value, string unit)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                TotalDataSentDisplay.UnitName = unit;
                TotalDataSentDisplay.Value = value;
            });
        }

        private void SetTotalDataReceived(double value, string unit)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                TotalDataReceivedDisplay.UnitName = unit;
                TotalDataReceivedDisplay.Value = value;
            });
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.TrayIconProvider.CreateTrayIcon("StatX");
            App.TrayIconProvider.NotifyIconClicked += TrayIconProvider_NotifyIconClicked;
            Application.Current.Exit += (s, a) =>
            {
                NetworkTracker.StopMonitoring();
            };

            MouseTracker = new MouseTracker();
            MouseTracker.ReloadStats();
            MouseTracker.DistanceTraveledChanged += MouseTracker_DistanceTraveledChanged;
            MouseTracker.DistanceScrolledChanged += MouseTracker_DistanceScrolledChanged;
            MouseTracker.ClicksChanged += MouseTracker_ClicksChanged;
            MouseTracker.LeftClicksChanged += MouseTracker_LeftClicksChanged;
            MouseTracker.RightClicksChanged += MouseTracker_RightClicksChanged;
            MouseTracker.MiddleClicksChanged += MouseTracker_MiddleClicksChanged;

            KeyboardTracker = new KeyboardTracker();
            KeyboardTracker.ReloadStats();
            KeyboardTracker.KeyPressesChanged += KeyboardTracker_KeyPressesChanged;
            KeyboardTracker.KeypressEnergyChanged += KeyboardTracker_KeyPressureChanged;


            NetworkTracker = new NetworkTracker();
            NetworkTracker.ReloadStats();
            NetworkTracker.SentDataChanged += NetworkTracker_SentDataChanged;
            NetworkTracker.ReceivedDataChanged += NetworkTracker_ReceivedDataChanged;
            NetworkTracker.StartMonitoring();

            KeyboardTracker_KeyPressesChanged(null, null);
            KeyboardTracker_KeyPressureChanged(null, null);
            MouseTracker_ClicksChanged(null, null);
            MouseTracker_DistanceScrolledChanged(null, null);
            MouseTracker_DistanceTraveledChanged(null, null);
            NetworkTracker_SentDataChanged(null, null);
        }

        private void TrayIconProvider_NotifyIconClicked(object sender, EventArgs eventArgs)
        {
            ToggleVisibility();
        }


        private void ToggleVisibility()
        {
            if (Visibility == Visibility.Visible)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        private void HideWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleVisibility();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            var choiceWindow = ChoiceWindow.ShowChoice("Really exit?", "Your statistics won't be tracked until you restart StatX!", this);
            choiceWindow.YesClicked += (s, ev) =>
            {
                Close();
            };
            choiceWindow.ShowDialog();
        }
    }
}
