using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OverCR.StatX
{
    /// <summary>
    /// Interaction logic for ChoiceWindow.xaml
    /// </summary>
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
