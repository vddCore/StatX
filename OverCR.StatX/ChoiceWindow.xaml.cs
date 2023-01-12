using System;
using System.Windows;

namespace OverCR.StatX
{
    public partial class ChoiceWindow
    {
        public event EventHandler YesClicked;
        public event EventHandler NoClicked;

        public ChoiceWindow()
        {
            InitializeComponent();
        }

        public static ChoiceWindow ShowChoice(string title, string message, Window owner)
        {
            var window = new ChoiceWindow
            {
                Title = title,
                MessageBlock = { Text = message },
                Owner = owner
            };
            window.Top = owner.Top + owner.Height / 2 - window.Height / 2;
            window.Left = owner.Left + owner.Width / 2 - window.Width / 2;

            return window;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            YesClicked?.Invoke(this, e);
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            NoClicked?.Invoke(this, e);
            Close();
        }
    }
}
