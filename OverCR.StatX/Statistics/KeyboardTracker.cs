using System;
using OverCR.StatX.Hooks.Keyboard;
using System.Collections.Generic;

namespace OverCR.StatX.Statistics
{
    class KeyboardTracker
    {
        public int TotalKeyPresses { get; private set; }
        public double TotalKeypressEnergy { get; private set; } // kilograms
        public Dictionary<string, long> KeySpecificStats { get; private set; }

        private KeyboardHook KeyboardHook { get; }

        public event EventHandler KeyPressesChanged;
        public event EventHandler KeyPressEnergyChanged;

        public KeyboardTracker()
        {
            KeyboardHook = new KeyboardHook();
            KeySpecificStats = new Dictionary<string, long>();

            KeyboardHook.KeyUp += KeyboardHook_KeyUp;
            KeyboardHook.KeyDown += KeyboardHook_KeyDown;
            KeyboardHook.Install();
        }

        public void ReloadStats()
        {
            TotalKeyPresses = App.StatisticsSaveFile.Section("Main").Entry<int>("TotalKeyPresses");
            TotalKeypressEnergy = App.StatisticsSaveFile.Section("Main").Entry<double>("TotalKeyPressEnergy");

            var keySpecificStats = App.StatisticsSaveFile.Section("KeySpecificStats");
            foreach (var kvp in keySpecificStats)
            {
                KeySpecificStats.Add(kvp.Key, keySpecificStats.Entry<long>(kvp.Key));
            }
        }

        private void KeyboardHook_KeyDown(KeyboardHookEventArgs e)
        {
            // membrane = ~0.075
            // scissor-switch = ~0.070
            // mechanical ~0.065
            TotalKeypressEnergy += 0.075 * 0.2;
            KeyPressEnergyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void KeyboardHook_KeyUp(KeyboardHookEventArgs e)
        {
            TotalKeyPresses += 1;

            UpdateKeySpecificStats(e.Key);
            KeyPressesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateKeySpecificStats(Keys key)
        {
            switch(key)
            {
                case Keys.Alpha0:
                case Keys.Alpha1:
                case Keys.Alpha2:
                case Keys.Alpha3:
                case Keys.Alpha4:
                case Keys.Alpha5:
                case Keys.Alpha6:
                case Keys.Alpha7:
                case Keys.Alpha8:
                case Keys.Alpha9:
                    var alphanum = key.ToString();
                    CreateOrIncrementKeySpecificStat("Alphanumeric " + alphanum[alphanum.Length - 1]);
                    break;
                case Keys.Minus:
                    CreateOrIncrementKeySpecificStat("Alphanumeric -");
                    break;
                case Keys.Plus:
                    CreateOrIncrementKeySpecificStat("Alphanumeric +");
                    break;
                case Keys.Num0:
                case Keys.Num1:
                case Keys.Num2:
                case Keys.Num3:
                case Keys.Num4:
                case Keys.Num5:
                case Keys.Num6:
                case Keys.Num7:
                case Keys.Num8:
                case Keys.Num9:
                    var numpad = key.ToString();
                    CreateOrIncrementKeySpecificStat("Numpad " + numpad[numpad.Length - 1]);
                    break;
                case Keys.NumSlash:
                    CreateOrIncrementKeySpecificStat("Numpad /");
                    break;
                case Keys.NumAsterisk:
                    CreateOrIncrementKeySpecificStat("Numpad *");
                    break;
                case Keys.NumMinus:
                    CreateOrIncrementKeySpecificStat("Numpad -");
                    break;
                case Keys.NumPlus:
                    CreateOrIncrementKeySpecificStat("Numpad +");
                    break;
                case Keys.NumComma:
                    CreateOrIncrementKeySpecificStat("Numpad Comma");
                    break;
                case Keys.LeftBracket:
                    CreateOrIncrementKeySpecificStat("Left Bracket");
                    break;
                case Keys.RightBracket:
                    CreateOrIncrementKeySpecificStat("Right Bracket");
                    break;
                case Keys.MediaPreviousTrack:
                    CreateOrIncrementKeySpecificStat("Previous Track");
                    break;
                case Keys.MediaNextTrack:
                    CreateOrIncrementKeySpecificStat("Next Track");
                    break;
                case Keys.MediaPlayPause:
                    CreateOrIncrementKeySpecificStat("Play/Pause");
                    break;
                case Keys.VolumeUp:
                    CreateOrIncrementKeySpecificStat("Volume Up");
                    break;
                case Keys.VolumeDown:
                    CreateOrIncrementKeySpecificStat("Volume Down");
                    break;
                case Keys.VolumeMute:
                    CreateOrIncrementKeySpecificStat("Volume Mute");
                    break;
                case Keys.LeftControl:
                    CreateOrIncrementKeySpecificStat("Left Control");
                    break;
                case Keys.RightControl:
                    CreateOrIncrementKeySpecificStat("Right Control");
                    break;
                case Keys.LeftShift:
                    CreateOrIncrementKeySpecificStat("Left Shift");
                    break;
                case Keys.RightShift:
                    CreateOrIncrementKeySpecificStat("Right Shift");
                    break;
                case Keys.LeftWindows:
                    CreateOrIncrementKeySpecificStat("Left Windows");
                    break;
                case Keys.RightWindows:
                    CreateOrIncrementKeySpecificStat("Right Windows");
                    break;
                case Keys.LeftAlt:
                    CreateOrIncrementKeySpecificStat("Left Alt");
                    break;
                case Keys.RightAlt:
                    CreateOrIncrementKeySpecificStat("Right Alt");
                    break;
                case Keys.ContextMenu:
                    CreateOrIncrementKeySpecificStat("Context Menu");
                    break;
                case Keys.LeftArrow:
                    CreateOrIncrementKeySpecificStat("Left Arrow");
                    break;
                case Keys.RightArrow:
                    CreateOrIncrementKeySpecificStat("Right Arrow");
                    break;
                case Keys.UpArrow:
                    CreateOrIncrementKeySpecificStat("Up Arrow");
                    break;
                case Keys.DownArrow:
                    CreateOrIncrementKeySpecificStat("Down Arrow");
                    break;
                case Keys.PrintScreen:
                    CreateOrIncrementKeySpecificStat("Print Screen");
                    break;
                case Keys.ScrollLock:
                    CreateOrIncrementKeySpecificStat("Scroll Lock");
                    break;
                case Keys.PauseBreak:
                    CreateOrIncrementKeySpecificStat("Break");
                    break;
                case Keys.PageUp:
                    CreateOrIncrementKeySpecificStat("Page Up");
                    break;
                case Keys.PageDown:
                    CreateOrIncrementKeySpecificStat("Page Down");
                    break;
                default:
                    CreateOrIncrementKeySpecificStat(key.ToString());
                    break;
            }
        }

        private void CreateOrIncrementKeySpecificStat(string key)
        {
            if (!KeySpecificStats.ContainsKey(key))
            {
                KeySpecificStats.Add(key, 1);
                App.StatisticsSaveFile.Section("KeySpecificStats").Add(key, KeySpecificStats[key].ToString());
            }
            else
            {
                KeySpecificStats[key]++;
                App.StatisticsSaveFile.Section("KeySpecificStats").SetEntryValue(key, KeySpecificStats[key].ToString());
            }
        }
    }
}
