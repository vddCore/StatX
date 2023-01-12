using System.Windows;

namespace OverCR.StatX
{
    public partial class AlertWindow
    {
        private AlertWindow()
        {
            InitializeComponent();
        }

        public static AlertWindow ErrorBox(string message, Window owner)
        {
            var window = new AlertWindow
            {
                Title = "An error occured",
                MessageBlock = { Text = message },
                Owner = owner
            };
            return window;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
