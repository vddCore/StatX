using System;
using OverCR.StatX.Hooks.Keyboard;

namespace OverCR.StatX.Statistics
{
    class KeyboardTracker
    {
        public int TotalKeyPresses { get; private set; }
        public double TotalKeypressEnergy { get; private set; } // kilograms

        private KeyboardHook KeyboardHook { get; }

        public event EventHandler KeyPressesChanged;
        public event EventHandler KeypressEnergyChanged;

        public KeyboardTracker()
        {
            KeyboardHook = new KeyboardHook();
            KeyboardHook.KeyUp += KeyboardHook_KeyUp;
            KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            KeyboardHook.Install();
        }

        public void ReloadStats()
        {
            int totalKeyPresses;
            double totalKeyPressEnergy;

            TotalKeyPresses = !int.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalKeyPresses"), out totalKeyPresses) ? 0 : totalKeyPresses;
            TotalKeypressEnergy = !double.TryParse(App.StatisticsSaveFile.Section("Main").Entry("TotalKeyPressEnergy"), out totalKeyPressEnergy) ? 0 : totalKeyPressEnergy;
        }

        private void KeyboardHook_KeyDown(KeyboardHookEventArgs e)
        {
            // membrane = ~0.075
            // scissor-switch = ~0.070
            // mechanical ~0.065
            TotalKeypressEnergy += 0.075 * 0.2;
            KeypressEnergyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void KeyboardHook_KeyUp(KeyboardHookEventArgs e)
        {
            TotalKeyPresses += 1;
            KeyPressesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
