using CommunityToolkit.Mvvm.ComponentModel;
using Shutdown_PC.Models.Enums;
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

        public static readonly DependencyProperty SetTimeValueProperty =
           DependencyProperty.Register
           (nameof(SetTimeValue),
            typeof(TimeSpan),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(TimeSpan.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SetTimerValueProperty =
          DependencyProperty.Register
          (nameof(SetTimerValue),
           typeof(TimeSpan),
           typeof(SetTimerControl),
            new FrameworkPropertyMetadata(TimeSpan.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TypeModificationProperty =
           DependencyProperty.Register
           (nameof(TypeModification),
            typeof(eTypeModification),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(eTypeModification.AfterTime, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onTypeModificationPPropertyChanged)));

        private int _hoursValue;

        private int _minutesValue;

        private int _secondsValue;


        public SetTimerControl()
        {
            InitializeComponent();

            HourUC.TimeValue = hoursValue;
            MinuteUC.TimeValue = minutesValue;
            SecondsUC.TimeValue = secondsValue;

            HourUC.TimeValueChanged += hourUC_TimeValueChanged;
            MinuteUC.TimeValueChanged += minuteUC_TimeValueChanged;
            SecondsUC.TimeValueChanged += secondsUC_TimeValueChanged;
        }

        private int hoursValue
        {
            get => _hoursValue;
            set
            {
                if (_hoursValue != value)
                {
                    _hoursValue = value;
                    onHorsValuePropertyChanged();
                }
            }
        }

        public eTypeModification TypeModification
        {
            get => (eTypeModification)GetValue(TypeModificationProperty);
            set => SetValue(TypeModificationProperty, value);
        }

        private int minutesValue
        {
            get => _minutesValue;
            set
            {
                if (_minutesValue != value)
                {
                    _minutesValue = value;
                    onMinutesValueChanged();
                }
            }
        }

        private int secondsValue
        {
            get => _secondsValue;
            set
            {
                if (_secondsValue != value)
                {
                    _secondsValue = value;
                    onSecondValueChanged();
                }
            }
        }

        public TimeSpan SetTimeValue
        {
            get => (TimeSpan)GetValue(SetTimeValueProperty);
            set
            {
                if (SetTimeValue != value)
                {
                    SetValue(SetTimeValueProperty, value);
                    distiobutionTimeValue();
                }
            }
        }

        public TimeSpan SetTimerValue
        {
            get => (TimeSpan)GetValue(SetTimerValueProperty);
            set
            {
                if (SetTimerValue != value)
                {
                    SetValue(SetTimerValueProperty, value);
                    distiobutionTimeValue();
                }
            }
        }

        private void distiobutionTimeValue()
        {
            hoursValue = SetTimeValue.Hours;
            minutesValue = SetTimeValue.Minutes;
            secondsValue = SetTimeValue.Seconds;
        }

        private static void onTypeModificationPPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onTypeModificationPropertyChanged(e);
        }

        private void controValuelMinutes()
        {
            var timeValue = minutesValue;
            var secondTimeValue = hoursValue;
            changeValues(ref timeValue, ref secondTimeValue, ref HourUC);
            minutesValue = timeValue;
            hoursValue = secondTimeValue;
        }
        private void controValuelSeconds()
        {
            var timeValue = secondsValue;
            var secondTimeValue = minutesValue;
            changeValues(ref timeValue, ref secondTimeValue, ref MinuteUC);
            secondsValue = timeValue;
            minutesValue = secondTimeValue;
        }

        private void hourUC_TimeValueChanged(object? sender, EventArgs e) => hoursValue = HourUC.TimeValue;

        private void changeValues(ref int timeValue, ref int secondTimeValue, ref NumericControl objNumCon)
        {
            if (timeValue == 60)
            {
                secondTimeValue++;
                objNumCon.TimeValue = secondTimeValue;
                timeValue = 0;
            }

            if (timeValue == -1 && secondTimeValue != 0)
            {
                secondTimeValue--;
                objNumCon.TimeValue = secondTimeValue;
                timeValue = 59;
            }
        }
        private void minuteUC_TimeValueChanged(object? sender, EventArgs e) => minutesValue = MinuteUC.TimeValue;


        private void onTypeModificationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            setTime();
        }
        private void onHorsValuePropertyChanged()
        {
            HourUC.TimeValue = hoursValue;
            setTime();
        }

        private void onMinutesValueChanged()
        {
            MinuteUC.PreviousValue = hoursValue;
            MinuteUC.TimeValue = minutesValue;
            controValuelMinutes();
            setTime();
        }

        private void onSecondValueChanged()
        {
            SecondsUC.PreviousValue = minutesValue;
            SecondsUC.TimeValue = secondsValue;
            controValuelSeconds();
            setTime();
        }

        private void secondsUC_TimeValueChanged(object? sender, EventArgs e) => secondsValue = SecondsUC.TimeValue;

        private void setTime()
        {
            if (minutesValue != 60 && minutesValue != -1 && secondsValue != 60 && secondsValue != -1)
            {
                var stringDateTime = string.Format($"{hoursValue}:{minutesValue}:{secondsValue}");
                SetTimeValue = TimeSpan.Parse(stringDateTime);
            }
        }

        private void setValuesUseControls()
        {

        }
    }
}
