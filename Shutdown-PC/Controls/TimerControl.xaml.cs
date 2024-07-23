using Shutdown_PC.Models.Enums;
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

        public static readonly DependencyProperty TypeModificationProperty =
               DependencyProperty.Register
               (nameof(TypeModification),
                typeof(eTypeModification),
                typeof(TimerControl),
                 new FrameworkPropertyMetadata(eTypeModification.AfterTime, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onTypeModificationPropertyChanged)));

        public TimerControl()
        {
            InitializeComponent();
        }

        public DateTime TimerValue
        {
            get => (DateTime)GetValue(TimerValueProperty);
            set => SetValue(TimerValueProperty, value);
        }
        public eTypeModification TypeModification
        {
            get => (eTypeModification)GetValue(TypeModificationProperty);
            set => SetValue(TypeModificationProperty, value);
        }


        private static void onTimerControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as TimerControl;
            uc.onTimerControlChanged(e);
        }

        private void onTimerControlChanged(DependencyPropertyChangedEventArgs e)
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    lblTimer.Content = TimerValue.ToString("HH:mm:ss");
                    lblDate.Content = TimerValue.ToShortDateString();
                    break;
                case eTypeModification.AfterTime:
                    lblTimer.Content = TimerValue.ToString("HH:mm:ss");
                    lblDate.Content = TimerValue.ToShortDateString();

                    var time = TimeSpan.FromSeconds(10);
                    lblTimer.Content = time.ToString();
                    lblDate.Content = "";
                    break;
            }

        }

        private static void onTypeModificationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as TimerControl;
            uc.onTypeModificationPropertyChanged(e);
        }

        private void onTypeModificationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
