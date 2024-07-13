using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Shutdown_PC.Controls
{
    /// <summary>
    /// Interaction logic for SetTimerControl.xaml
    /// </summary>
    public partial class SetTimerControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SetTimeValueProperty =
           DependencyProperty.Register
           (nameof(SetTimeValue),
            typeof(TimeSpan),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(TimeSpan.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public static readonly DependencyProperty MinutesValueProperty =
        //   DependencyProperty.Register
        //   (nameof(MinutesValue),
        //    typeof(int),
        //    typeof(SetTimerControl),
        //     new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public static readonly DependencyProperty SecondsValueProperty =
        //   DependencyProperty.Register
        //   (nameof(SecondsValue),
        //    typeof(int),
        //    typeof(SetTimerControl),
        //     new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public SetTimerControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TimeSpan SetTimeValue
        {
            get => (TimeSpan)GetValue(SetTimeValueProperty);
            set => SetValue(SetTimeValueProperty, value);
        }
        //public int MinutesValue
        //{
        //    get => (int)GetValue(MinutesValueProperty);
        //    set => SetValue(MinutesValueProperty, value);
        //}
        //public int SecondsValue
        //{
        //    get => (int)GetValue(SecondsValueProperty);
        //    set => SetValue(SecondsValueProperty, value);
        //}
        private int _hoursValue;
        public int HoursValue
        {
            get => _hoursValue;
            set
            {
                _hoursValue = value;
                setTime();
                OnPropertyChanged(nameof(HoursValue));
            }
        }
        private int _minutesValue;
        public int MinutesValue
        {
            get => _minutesValue;
            set
            {
                _minutesValue = value;
                setTime();
                OnPropertyChanged(nameof(MinutesValue));
            }
        }
        private int _secondsValue;
        public int SecondsValue
        {
            get => _secondsValue;
            set
            {
                _secondsValue = value;
                //controValuelSeconds();
                // setTime();
                OnPropertyChanged(nameof(SecondsValue));
            }
        }

        private void controValuelSeconds()
        {
            if (SecondsValue == 60)
            {
                MinutesValue++;
                SecondsValue = 0;
            }
        }

        private void setTime()
        {
            if (MinutesValue != 60 && MinutesValue != -1 && SecondsValue != 60 && SecondsValue != -1)
            {
                var stringDateTime = string.Format($"{HoursValue}:{MinutesValue}:{SecondsValue}");
                //SetTimeValue = TimeSpan.Parse(stringDateTime);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private static void onHorsValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var uc = d as SetTimerControl;
        //    uc.onHorsValuePropertyChanged(e);
        //}

        //private static void ononMinutesValueChangedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var uc = d as SetTimerControl;
        //    uc.ononMinutesValueChangedChanged(e);
        //}

        //private static void onSecondValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var uc = d as SetTimerControl;
        //    uc.onSecondValueChanged(e);
        //}

        //private void onHorsValuePropertyChanged(DependencyPropertyChangedEventArgs e)
        //{

        //}
        //private void ononMinutesValueChangedChanged(DependencyPropertyChangedEventArgs e)
        //{

        //}
        //private void onSecondValueChanged(DependencyPropertyChangedEventArgs e)
        //{

        //}
    }
}
