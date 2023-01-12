using System;
using System.Windows;
using OverCR.StatX.Statistics;

namespace OverCR.StatX
{
    public partial class MainWindow
    {
        private MouseTracker MouseTracker { get; set; }
        private KeyboardTracker KeyboardTracker { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void KeyboardTracker_KeyPressesChanged(object sender, EventArgs e)
        {
            TotalKeyPressesDisplay.Value = KeyboardTracker.TotalKeyPresses;
        }

        private void KeyboardTracker_KeyPressureChanged(object sender, EventArgs e)
        {
            TotalKeyPressureDisplay.Value = Math.Round(KeyboardTracker.TotalKeypressEnergy, 2);
        }

        private void MouseTracker_DistanceTraveledChanged(object sender, EventArgs e)
        {
            TotalMouseDistanceDisplay.Value = Math.Round(MouseTracker.TotalDistanceTraveled, 2);
        }

        private void MouseTracker_DistanceScrolledChanged(object sender, EventArgs e)
        {
            TotalMouseScrollDistanceDisplay.Value = Math.Round(MouseTracker.TotalDistanceScrolled, 2);
        }

        private void MouseTracker_ClicksChanged(object sender, EventArgs e)
        {
            TotalMouseClicksDisplay.Value = MouseTracker.TotalClicks;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.NotifyIconClicked += App_NotifyIconClicked;

            MouseTracker = new MouseTracker();
            MouseTracker.DistanceTraveledChanged += MouseTracker_DistanceTraveledChanged;
            MouseTracker.DistanceScrolledChanged += MouseTracker_DistanceScrolledChanged;
            MouseTracker.ClicksChanged += MouseTracker_ClicksChanged;

            KeyboardTracker = new KeyboardTracker();
            KeyboardTracker.KeyPressesChanged += KeyboardTracker_KeyPressesChanged;
            KeyboardTracker.KeypressEnergyChanged += KeyboardTracker_KeyPressureChanged;
        }

        private void App_NotifyIconClicked(object sender, EventArgs e)
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
    }
}
