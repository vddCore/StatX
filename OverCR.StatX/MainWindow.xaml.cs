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
            App.StatisticsSaveFile.SetValue("TotalKeyPresses", KeyboardTracker.TotalKeyPresses);
        }

        private void KeyboardTracker_KeyPressureChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.SetValue("TotalKeyPressEnergy", KeyboardTracker.TotalKeypressEnergy);
        }

        private void MouseTracker_DistanceTraveledChanged(object sender, EventArgs e)
        {
            TotalMouseDistanceDisplay.Value = Math.Round(MouseTracker.TotalDistanceTraveled, 2);
            App.StatisticsSaveFile.SetValue("TotalMouseTravelDistance", MouseTracker.TotalDistanceTraveled);
        }

        private void MouseTracker_DistanceScrolledChanged(object sender, EventArgs e)
        {
            TotalMouseScrollDistanceDisplay.Value = Math.Round(MouseTracker.TotalDistanceScrolled, 2);
            App.StatisticsSaveFile.SetValue("TotalMouseScrollDistance", MouseTracker.TotalDistanceScrolled);
        }

        private void MouseTracker_MiddleClicksChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.SetValue("TotalMouseMiddleClicks", MouseTracker.TotalMiddleClicks);
        }

        private void MouseTracker_RightClicksChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.SetValue("TotalMouseRightClicks", MouseTracker.TotalRightClicks);
        }

        private void MouseTracker_LeftClicksChanged(object sender, EventArgs e)
        {
            App.StatisticsSaveFile.SetValue("TotalMouseLeftClicks", MouseTracker.TotalLeftClicks);
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
                    App.StatisticsSaveFile.SetValue("TotalDataSent", teraBytes);
                    App.StatisticsSaveFile.SetValue("TotalDataSentUnit", "terabytes");
                }
                else
                {
                    SetTotalDataSent(gigaBytes, "gigabytes");
                    App.StatisticsSaveFile.SetValue("TotalDataSent", gigaBytes);
                    App.StatisticsSaveFile.SetValue("TotalDataSentUnit", "gigabytes");
                }
            }
            else
            {
                SetTotalDataSent(megaBytes, "megabytes");
                App.StatisticsSaveFile.SetValue("TotalDataSent", megaBytes);
                App.StatisticsSaveFile.SetValue("TotalDataSentUnit", "megabytes");
            }
            App.StatisticsSaveFile.SetValue("TotalBytesSent", NetworkTracker.TotalBytesSent);
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
                    App.StatisticsSaveFile.SetValue("TotalDataReceived", teraBytes);
                    App.StatisticsSaveFile.SetValue("TotalDataReceivedUnit", "terabytes");
                }
                else
                {
                    SetTotalDataReceived(gigaBytes, "gigabytes");
                    App.StatisticsSaveFile.SetValue("TotalDataReceived", gigaBytes);
                    App.StatisticsSaveFile.SetValue("TotalDataReceivedUnit", "gigabytes");
                }
            }
            else
            {
                SetTotalDataReceived(megaBytes, "megabytes");
                App.StatisticsSaveFile.SetValue("TotalDataReceived", megaBytes);
                App.StatisticsSaveFile.SetValue("TotalDataReceivedUnit", "megabytes");
            }
            App.StatisticsSaveFile.SetValue("TotalBytesReceived", NetworkTracker.TotalBytesReceived);
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

            NetworkTracker = new NetworkTracker();
            NetworkTracker.ReloadStats();
            NetworkTracker.SentDataChanged += NetworkTracker_SentDataChanged;
            NetworkTracker.ReceivedDataChanged += NetworkTracker_ReceivedDataChanged;
            NetworkTracker.StartMonitoring();

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
            KeyboardTracker.KeyPressEnergyChanged += KeyboardTracker_KeyPressureChanged;


            KeyboardTracker_KeyPressesChanged(null, null);
            KeyboardTracker_KeyPressureChanged(null, null);
            MouseTracker_ClicksChanged(null, null);
            MouseTracker_DistanceScrolledChanged(null, null);
            MouseTracker_DistanceTraveledChanged(null, null);
            NetworkTracker_SentDataChanged(null, null);
            NetworkTracker_ReceivedDataChanged(null, null);
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
                Activate();
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