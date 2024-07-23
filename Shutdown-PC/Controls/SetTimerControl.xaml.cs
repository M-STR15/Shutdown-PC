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


        private int _hoursValue { get; set; }

        private int _minutesValue { get; set; }

        private int _secondsValue { get; set; }


        private int _timerHoursValue { get; set; }

        private int _timerMinutesValue;

        private int _timerSecondsValue;


        public SetTimerControl()
        {
            InitializeComponent();

            HoursUC.TimeValue = timerHoursValue;
            MinutesUC.TimeValue = timerMinutesValue;
            SecondsUC.TimeValue = timerSecondsValue;

            HoursUC.TimeValueChanged += hourUC_TimeValueChanged;
            MinutesUC.TimeValueChanged += minuteUC_TimeValueChanged;
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
            get => _timerHoursValue;
            set
            {
                if (_timerHoursValue != value)
                {
                    _timerHoursValue = value;
                    onHoursValuePropertyChanged();
                }
            }
        }
        private int timerMinutesValue
        {
            get => _timerMinutesValue;
            set
            {
                if (_timerMinutesValue != value)
                {
                    _timerMinutesValue = value;
                    onMinutesValueChanged();
                }
            }
        }

        private int timerSecondsValue
        {
            get => _timerSecondsValue;
            set
            {
                if (_timerSecondsValue != value)
                {
                    _timerSecondsValue = value;
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
            var timeValue = _minutesValue;
            var secondTimeValue = _hoursValue;
            changeValues(ref timeValue, ref secondTimeValue, ref HoursUC);
            _minutesValue = timeValue;
            _hoursValue = secondTimeValue;
        }

        private void controValuelSeconds()
        {
            var timeValue = _secondsValue;
            var secondTimeValue = _minutesValue;
            changeValues(ref timeValue, ref secondTimeValue, ref MinutesUC);
            _secondsValue = timeValue;
            _minutesValue = secondTimeValue;
        }

        private void distiobutionTimeValue()
        {
            //hoursValue = SetTimeValue.Hour;
            //minutesValue = SetTimeValue.Minute;
            //secondsValue = SetTimeValue.Second;
        }
        private void hourUC_TimeValueChanged(object? sender, EventArgs e)
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    timerHoursValue = HoursUC.TimeValue;
                    //_hoursValue = HourUC.TimeValue - _editDatetime.Hour;
                    break;
                case eTypeModification.AfterTime:
                    //timerHoursValue = _editDatetime.AddHours(HourUC.TimeValue).Hour;
                    _hoursValue = HoursUC.TimeValue;
                    break;
            }
        }

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
        private void minuteUC_TimeValueChanged(object? sender, EventArgs e)
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    timerMinutesValue = MinutesUC.TimeValue;
                    _minutesValue = _editDatetime.Minute;
                    //_hoursValue = HourUC.TimeValue - _editDatetime.Hour;
                    break;
                case eTypeModification.AfterTime:
                    //timerHoursValue = _editDatetime.AddHours(HourUC.TimeValue).Hour;
                    _minutesValue = MinutesUC.TimeValue;
                    break;
            }
        }

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
                    HoursUC.TimeValue = SetTimeValue.Hour;
                    break;
                case eTypeModification.AfterTime:
                    HoursUC.TimeValue = _hoursValue;
                    break;
            }
        }
        private void setMinutesUC()
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    MinutesUC.PreviousValue = SetTimeValue.Hour;
                    MinutesUC.TimeValue = SetTimeValue.Minute;
                    break;
                case eTypeModification.AfterTime:
                    MinutesUC.PreviousValue = _hoursValue;
                    MinutesUC.TimeValue = _minutesValue;
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
                    SecondsUC.PreviousValue = _minutesValue;
                    SecondsUC.TimeValue = _secondsValue;
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
        private void secondsUC_TimeValueChanged(object? sender, EventArgs e)
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    timerSecondsValue = SecondsUC.TimeValue;
                    //_hoursValue = HourUC.TimeValue - _editDatetime.Hour;
                    break;
                case eTypeModification.AfterTime:
                    //timerHoursValue = _editDatetime.AddHours(HourUC.TimeValue).Hour;
                    _secondsValue = SecondsUC.TimeValue;
                    break;
            }
        }

        private void setTime()
        {
            if (timerMinutesValue != 60 && timerMinutesValue > -1 && timerSecondsValue != 60 && timerSecondsValue > -1)
            {
                SetTimeValue = _editDatetime.AddHours(_hoursValue).AddMinutes(_minutesValue).AddSeconds(_secondsValue);
            }
        }

        private void setTimer()
        {
            SetTimerValue = new TimeSpan(timerHoursValue, timerMinutesValue, timerSecondsValue);
        }
    }
}
