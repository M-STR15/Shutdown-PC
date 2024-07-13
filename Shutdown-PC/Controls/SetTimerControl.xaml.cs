using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Shutdown_PC.Controls
{
    /// <summary>
    /// Interaction logic for SetTimerControl.xaml
    /// </summary>
    [ObservableObject]
    public partial class SetTimerControl : UserControl
    {
        public static readonly DependencyProperty HoursValueProperty =
          DependencyProperty.Register
          (nameof(HoursValue),
           typeof(int),
           typeof(SetTimerControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onHorsValuePropertyChanged)));

        public static readonly DependencyProperty MinutesValueProperty =
          DependencyProperty.Register
          (nameof(MinutesValue),
           typeof(int),
           typeof(SetTimerControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onMinutesValueChangedChanged)));

        public static readonly DependencyProperty SecondsValueProperty =
           DependencyProperty.Register
           (nameof(SecondsValue),
            typeof(int),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onSecondValueChanged)));

        public static readonly DependencyProperty SetTimeValueProperty =
                                   DependencyProperty.Register
           (nameof(SetTimeValue),
            typeof(TimeSpan),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(TimeSpan.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        private int _hoursValue;

        private int _minutesValue;

        private int _secondsValue2;

        public SetTimerControl()
        {
            InitializeComponent();

            HourUC.TimeValue = HoursValue;
            MinuteUC.TimeValue = MinutesValue;
            SecondsUC.TimeValue = SecondsValue;

            HourUC.TimeValueChanged += hourUC_TimeValueChanged;
            MinuteUC.TimeValueChanged += minuteUC_TimeValueChanged;
            SecondsUC.TimeValueChanged += secondsUC_TimeValueChanged;
        }

        public int HoursValue
        {
            get => (int)GetValue(HoursValueProperty);
            set => SetValue(HoursValueProperty, value);
        }

        public int MinutesValue
        {
            get => (int)GetValue(MinutesValueProperty);
            set => SetValue(MinutesValueProperty, value);
        }

        public int SecondsValue
        {
            get => (int)GetValue(SecondsValueProperty);
            set => SetValue(SecondsValueProperty, value);
        }

        public int SecondsValue2
        {
            get => _secondsValue2;
            set
            {
                if (_secondsValue2 != value)
                {
                    _secondsValue2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan SetTimeValue
        {
            get => (TimeSpan)GetValue(SetTimeValueProperty);
            set => SetValue(SetTimeValueProperty, value);
        }

        private static void onHorsValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onHorsValuePropertyChanged(e);
        }

        private static void onMinutesValueChangedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onMinutesValueChangedChanged(e);
        }

        private static void onSecondValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onSecondValueChanged(e);
        }

        private void controValuelSeconds()
        {
            if (SecondsValue == 60)
            {
                MinutesValue++;
                SecondsValue = 0;
            }
        }

        private void hourUC_TimeValueChanged(object? sender, EventArgs e)
        {
            HoursValue = HourUC.TimeValue;
        }

        private void minuteUC_TimeValueChanged(object? sender, EventArgs e)
        {
            MinutesValue = MinuteUC.TimeValue;
        }

        private void onHorsValuePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            HourUC.TimeValue = MinutesValue;
            setTime();
        }

        private void onMinutesValueChangedChanged(DependencyPropertyChangedEventArgs e)
        {
            MinuteUC.PreviousValue = HoursValue;
            MinuteUC.TimeValue = MinutesValue;
            setTime();
        }

        private void onSecondValueChanged(DependencyPropertyChangedEventArgs e)
        {
            SecondsUC.PreviousValue = MinutesValue;
            SecondsUC.TimeValue = SecondsValue;
            setTime();
        }

        private void secondsUC_TimeValueChanged(object? sender, EventArgs e)
        {
            SecondsValue = SecondsUC.TimeValue;
        }
        private void setTime()
        {
            if (MinutesValue != 60 && MinutesValue != -1 && SecondsValue != 60 && SecondsValue != -1)
            {
                var stringDateTime = string.Format($"{HoursValue}:{MinutesValue}:{SecondsValue}");
                SetTimeValue = TimeSpan.Parse(stringDateTime);
            }
        }
    }
}
