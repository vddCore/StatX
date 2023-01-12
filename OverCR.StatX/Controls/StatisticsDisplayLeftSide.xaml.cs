using System.Windows;

namespace OverCR.StatX.Controls
{
    public partial class StatisticsDisplayLeftSide
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public string UnitName
        {
            get { return (string)GetValue(UnitNameProperty); }
            set
            {
                SetValue(UnitNameProperty, value);
            }
        }

        public double Value
        {
            get { return (float)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(StatisticsDisplayLeftSide));
        public static readonly DependencyProperty UnitNameProperty = DependencyProperty.Register("UnitName", typeof(string), typeof(StatisticsDisplayLeftSide));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(StatisticsDisplayLeftSide));

        public StatisticsDisplayLeftSide()
        {
            InitializeComponent();
        }
    }
}
