using System;
using OverCR.StatX.Hooks.Mouse;

namespace OverCR.StatX.Statistics
{
    class MouseTracker
    {
        public double TotalDistanceTraveled { get; private set; } // Meters
        public double TotalDistanceScrolled { get; private set; }
        public int TotalLeftClicks { get; private set; }
        public int TotalMiddleClicks { get; private set; }
        public int TotalRightClicks { get; private set; }
        public int TotalClicks => (TotalLeftClicks + TotalMiddleClicks + TotalRightClicks);

        private MouseHook MouseHook { get; }

        private int PrevX { get; set; }
        private int PrevY { get; set; }

        public event EventHandler DistanceTraveledChanged;
        public event EventHandler DistanceScrolledChanged;
        public event EventHandler LeftClicksChanged;
        public event EventHandler MiddleClicksChanged;
        public event EventHandler RightClicksChanged;
        public event EventHandler ClicksChanged;

        public MouseTracker()
        {
            MouseHook = new MouseHook();
            MouseHook.MouseMove += MouseHook_MouseMove;
            MouseHook.MouseScroll += MouseHook_MouseScroll;
            MouseHook.LeftMouseButtonDown += MouseHook_LeftMouseButtonDown;
            MouseHook.MiddleMouseButtonDown += MouseHook_MiddleMouseButtonDown;
            MouseHook.RightMouseButtonDown += MouseHook_RightMouseButtonDown;

            MouseHook.Install();
        }

        public void ReloadStats()
        {
            int totalLeftClicks;
            int totalRightClicks;
            int totalMiddleClicks;
            double totalDistanceTraveled;
            double totalDistanceScrolled;

            TotalLeftClicks = !int.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalMouseLeftClicks"), out totalLeftClicks) ? 0 : totalLeftClicks;
            TotalRightClicks = !int.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalMouseRightClicks"), out totalRightClicks) ? 0 : totalRightClicks;
            TotalMiddleClicks = !int.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalMouseMiddleClicks"), out totalMiddleClicks) ? 0 : totalMiddleClicks;
            TotalDistanceTraveled = !double.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalMouseTravelDistance"), out totalDistanceTraveled) ? 0 : totalDistanceTraveled;
            TotalDistanceScrolled = !double.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalMouseScrollDistance"), out totalDistanceScrolled) ? 0 : totalDistanceScrolled;
        }

        private void MouseHook_LeftMouseButtonDown(MouseHookEventArgs e)
        {
            TotalLeftClicks += 1;
            LeftClicksChanged?.Invoke(this, EventArgs.Empty);

            ClicksChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MouseHook_MiddleMouseButtonDown(MouseHookEventArgs e)
        {
            TotalMiddleClicks += 1;
            MiddleClicksChanged?.Invoke(this, EventArgs.Empty);

            ClicksChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MouseHook_RightMouseButtonDown(MouseHookEventArgs e)
        {
            TotalRightClicks += 1;
            RightClicksChanged?.Invoke(this, EventArgs.Empty);

            ClicksChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MouseHook_MouseMove(MouseHookEventArgs e)
        {
            // 1px = 0.2mm
            if (PrevX == -1 || PrevY == -1)
            {
                PrevX = e.X;
                PrevY = e.Y;
                return;
            }
            var distance = Math.Sqrt(Math.Pow(e.X - PrevX, 2) + Math.Pow(e.Y - PrevY, 2));
            distance *= 0.0002; // Meters

            TotalDistanceTraveled += distance;
            DistanceTraveledChanged?.Invoke(this, EventArgs.Empty);

            PrevX = e.X;
            PrevY = e.Y;
        }

        private void MouseHook_MouseScroll(MouseHookEventArgs e)
        {
            TotalDistanceScrolled += 0.00432; // Meters
            DistanceScrolledChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
