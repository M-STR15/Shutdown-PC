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

        public static readonly DependencyProperty SetTimerValueProperty =
          DependencyProperty.Register
          (nameof(SetTimerValue),
           typeof(TimeSpan),
           typeof(SetTimerControl),
            new FrameworkPropertyMetadata(TimeSpan.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SetTimeValueProperty =
           DependencyProperty.Register
           (nameof(SetTimeValue),
            typeof(DateTime),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TypeModificationProperty =
           DependencyProperty.Register
           (nameof(TypeModification),
            typeof(eTypeModification),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(eTypeModification.AfterTime, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onTypeModificationPropertyChanged)));



        private int _hoursValue;

        private int _minutesValue;

        private int _secondsValue;


        public SetTimerControl()
        {
            InitializeComponent();

            HourUC.TimeValue = timerHoursValue;
            MinuteUC.TimeValue = timerMinutesValue;
            SecondsUC.TimeValue = timerSecondsValue;

            HourUC.TimeValueChanged += hourUC_TimeValueChanged;
            MinuteUC.TimeValueChanged += minuteUC_TimeValueChanged;
            SecondsUC.TimeValueChanged += secondsUC_TimeValueChanged;

            _editDatetime = DateTime.Now;

            setTime();
            setTimer();

            setHoursUC();
            setMinutesUC();
            setSeconcsUC();
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

        public DateTime SetTimeValue
        {
            get => (DateTime)GetValue(SetTimeValueProperty);
            set
            {
                if (SetTimeValue != value)
                {
                    SetValue(SetTimeValueProperty, value);
                    distiobutionTimeValue();
                }
            }
        }

        public eTypeModification TypeModification
        {
            get => (eTypeModification)GetValue(TypeModificationProperty);
            set => SetValue(TypeModificationProperty, value);
        }

        private DateTime _editDatetime;

        private int timerHoursValue
        {
            get => _hoursValue;
            set
            {
                if (_hoursValue != value)
                {
                    _hoursValue = value;
                    onHoursValuePropertyChanged();
                }
            }
        }
        private int timerMinutesValue
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

        private int timerSecondsValue
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
        private static void onTypeModificationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onTypeModificationPropertyChanged(e);
        }

        private void controValuelMinutes()
        {
            var timeValue = timerMinutesValue;
            var secondTimeValue = timerHoursValue;
            changeValues(ref timeValue, ref secondTimeValue, ref HourUC);
            timerMinutesValue = timeValue;
            timerHoursValue = secondTimeValue;
        }

        private void controValuelSeconds()
        {
            var timeValue = timerSecondsValue;
            var secondTimeValue = timerMinutesValue;
            changeValues(ref timeValue, ref secondTimeValue, ref MinuteUC);
            timerSecondsValue = timeValue;
            timerMinutesValue = secondTimeValue;
        }

        private void distiobutionTimeValue()
        {
            //hoursValue = SetTimeValue.Hour;
            //minutesValue = SetTimeValue.Minute;
            //secondsValue = SetTimeValue.Second;
        }
        private void hourUC_TimeValueChanged(object? sender, EventArgs e) => timerHoursValue = HourUC.TimeValue;

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
        private void minuteUC_TimeValueChanged(object? sender, EventArgs e) => timerMinutesValue = MinuteUC.TimeValue;


        private void onHoursValuePropertyChanged()
        {
            setHoursUC();
            setTime();
            setTimer();
        }

        private void onMinutesValueChanged()
        {
            setMinutesUC();
            controValuelMinutes();
            setTime();
            setTimer();
        }

        private void onSecondValueChanged()
        {
            setSeconcsUC();
            controValuelSeconds();
            setTime();
            setTimer();
        }

        private void setHoursUC()
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    HourUC.TimeValue = SetTimeValue.Hour;
                    break;
                case eTypeModification.AfterTime:
                    HourUC.TimeValue = timerHoursValue;
                    break;
            }
        }
        private void setMinutesUC()
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    MinuteUC.PreviousValue = SetTimeValue.Hour;
                    MinuteUC.TimeValue = SetTimeValue.Minute;
                    break;
                case eTypeModification.AfterTime:
                    MinuteUC.PreviousValue = timerHoursValue;
                    MinuteUC.TimeValue = timerMinutesValue;
                    break;
            }
        }
        private void setSeconcsUC()
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    SecondsUC.PreviousValue = SetTimeValue.Minute;
                    SecondsUC.TimeValue = SetTimeValue.Second;
                    break;
                case eTypeModification.AfterTime:
                    SecondsUC.PreviousValue = timerMinutesValue;
                    SecondsUC.TimeValue = timerSecondsValue;
                    break;
            }
        }

        private void onTypeModificationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            setTime();
            setTimer();

            setHoursUC();
            setMinutesUC();
            setSeconcsUC();
        }
        private void secondsUC_TimeValueChanged(object? sender, EventArgs e) => timerSecondsValue = SecondsUC.TimeValue;

        private void setTime()
        {
            if (timerMinutesValue != 60 && timerMinutesValue > -1 && timerSecondsValue != 60 && timerSecondsValue > -1)
            {
                SetTimeValue = _editDatetime.AddHours(timerHoursValue).AddMinutes(timerMinutesValue).AddSeconds(timerSecondsValue);
            }
        }

        private void setTimer()
        {
            SetTimerValue = new TimeSpan(timerHoursValue, timerMinutesValue, timerSecondsValue);
        }
    }
}
