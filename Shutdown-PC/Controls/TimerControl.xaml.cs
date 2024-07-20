using System.Windows;
using System.Windows.Controls;

namespace Shutdown_PC.Controls
{
    /// <summary>
    /// Interaction logic for TimerControl.xaml
    /// </summary>
    public partial class TimerControl : UserControl
    {
        public static readonly DependencyProperty TimerValueProperty =
              DependencyProperty.Register
              (nameof(TimerValue),
               typeof(DateTime),
               typeof(TimerControl), new PropertyMetadata(new PropertyChangedCallback(onTimerControlChanged)));

        public TimerControl()
        {
            InitializeComponent();
        }

        public DateTime TimerValue
        {
            get => (DateTime)GetValue(TimerValueProperty);
            set => SetValue(TimerValueProperty, value);
        }

        private static void onTimerControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as TimerControl;
            uc.onTimerControlChanged(e);
        }

        private void onTimerControlChanged(DependencyPropertyChangedEventArgs e)
        {
            lblTimer.Content = TimerValue.ToShortTimeString();
        }
    }
}
