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

            HourUC.TimeValue = hoursValue;
            MinuteUC.TimeValue = minutesValue;
            SecondsUC.TimeValue = secondsValue;

            HourUC.TimeValueChanged += hourUC_TimeValueChanged;
            MinuteUC.TimeValueChanged += minuteUC_TimeValueChanged;
            SecondsUC.TimeValueChanged += secondsUC_TimeValueChanged;

            _editDatetime = DateTime.Now;

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

        private int hoursValue
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
        private static void onTypeModificationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        private void distiobutionTimeValue()
        {
            //hoursValue = SetTimeValue.Hour;
            //minutesValue = SetTimeValue.Minute;
            //secondsValue = SetTimeValue.Second;
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
                    HourUC.TimeValue = hoursValue;
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
                    MinuteUC.PreviousValue = hoursValue;
                    MinuteUC.TimeValue = minutesValue;
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
                    SecondsUC.PreviousValue = minutesValue;
                    SecondsUC.TimeValue = secondsValue;
                    break;
            }
        }

        private void onTypeModificationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            setTime();
            setTimer();
        }
        private void secondsUC_TimeValueChanged(object? sender, EventArgs e) => secondsValue = SecondsUC.TimeValue;

        private void setTime()
        {
            if (minutesValue != 60 && minutesValue != -1 && secondsValue != 60 && secondsValue != -1)
            {
                SetTimeValue = _editDatetime.AddHours(hoursValue).AddMinutes(minutesValue).AddSeconds(secondsValue);
            }
        }

        private void setTimer()
        {
            SetTimerValue = new TimeSpan(hoursValue, minutesValue, secondsValue);
        }

        private void setValuesUseControls()
        {
            _editDatetime = DateTime.Now;
            switch (TypeModification)
            {
                case eTypeModification.InTime:

                    break;
                case eTypeModification.AfterTime:

                    break;
            }
        }
    }
}
