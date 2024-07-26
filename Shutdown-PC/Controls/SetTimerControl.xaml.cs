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
    public partial class SetTimerControl : UserControl, IDisposable
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

        public static readonly DependencyProperty StatusProperty =
           DependencyProperty.Register
           (nameof(Status),
            typeof(eStatus),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(eStatus.Run,new PropertyChangedCallback(onStatusPropertyChanged)));

        public static readonly DependencyProperty EndAfterSecondsProperty =
           DependencyProperty.Register
           (nameof(EndAfterSeconds),
            typeof(int),
            typeof(SetTimerControl),
             new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(onEndAfterSecondsPropertyChanged)));

        public int EndAfterSeconds
        {
            get => (int)GetValue(EndAfterSecondsProperty);
            set
            {
                SetValue(EndAfterSecondsProperty, value);
                SetTimeValue =_editDatetime.AddSeconds(value);
            }
        }


        public eStatus Status
        {
            get => (eStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public SetTimerControl()
        {
            InitializeComponent();

            HoursUC.btnPlus.Click += hoursPlus_Change;
            HoursUC.btnMinus.Click += hoursMinus_Change;

            MinutesUC.btnPlus.Click += minutesPlus_Change;
            MinutesUC.btnMinus.Click += minutesMinus_Change;

            SecondsUC.btnPlus.Click += secondsPlus_Change;
            SecondsUC.btnMinus.Click += secondsMinus_Change;

            _editDatetime = DateTime.Now;

        }

        private void hoursPlus_Change(object sender, EventArgs args)
        {
            EndAfterSeconds += 60 * 60;
            setUCNumeric();
        }

        private void hoursMinus_Change(object sender, EventArgs args)
        {
            EndAfterSeconds -= 60 * 60;
            setUCNumeric();
        }

        private void minutesPlus_Change(object sender, EventArgs args)
        {
            EndAfterSeconds += 60;
            setUCNumeric();
        }

        private void minutesMinus_Change(object sender, EventArgs args)
        {
            EndAfterSeconds -= 60;
            setUCNumeric();
        }

        private void secondsPlus_Change(object sender, EventArgs args)
        {
            EndAfterSeconds += 1;
            setUCNumeric();
        }

        private void secondsMinus_Change(object sender, EventArgs args)
        {
            EndAfterSeconds -= 1;
            setUCNumeric();
        }

        private void setUCNumeric()
        {

            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    var finalDatetime = _editDatetime.AddSeconds(EndAfterSeconds);
                    HoursUC.TimeValue = finalDatetime.Hour;
                    MinutesUC.TimeValue = finalDatetime.Minute;
                    SecondsUC.TimeValue = finalDatetime.Second;

                    lblDate.Content = finalDatetime.ToShortDateString();
                    break;
                case eTypeModification.AfterTime:
                    var time = TimeSpan.FromSeconds(EndAfterSeconds);
                    HoursUC.TimeValue = (int)time.TotalHours;
                    MinutesUC.TimeValue = time.Minutes;
                    SecondsUC.TimeValue = time.Seconds;

                    lblDate.Content = "";
                    break;
            }
        }

        public TimeSpan SetTimerValue
        {
            get => (TimeSpan)GetValue(SetTimerValueProperty);
            set
            {
                if (SetTimerValue != value)
                    SetValue(SetTimerValueProperty, value);
            }
        }

        public DateTime SetTimeValue
        {
            get => (DateTime)GetValue(SetTimeValueProperty);
            set
            {
                if (SetTimeValue != value)
                    SetValue(SetTimeValueProperty, value);
            }
        }

        public eTypeModification TypeModification
        {
            get => (eTypeModification)GetValue(TypeModificationProperty);
            set => SetValue(TypeModificationProperty, value);
        }

        private DateTime _editDatetime;

        private static void onTypeModificationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onTypeModificationPropertyChanged(e);
        }

        private void onTypeModificationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            setUCNumeric();
        }

        private static void onEndAfterSecondsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onEndAfterSecondsPropertyChanged(e);
        }

        private void onEndAfterSecondsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            setUCNumeric();
        }




        private static void onStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onStatusPropertyChanged(e);
        }

        private void onStatusPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            var vis = Visibility.Visible;
            if (Status == eStatus.Run)
                vis = Visibility.Hidden;

            HoursUC.VisibilityButtons = vis;
            MinutesUC.VisibilityButtons = vis;
            SecondsUC.VisibilityButtons = vis;
        }

        public void Dispose()
        {
            HoursUC.btnPlus.Click -= hoursPlus_Change;
            HoursUC.btnMinus.Click -= hoursMinus_Change;

            MinutesUC.btnPlus.Click -= minutesPlus_Change;
            MinutesUC.btnMinus.Click -= minutesMinus_Change;

            SecondsUC.btnPlus.Click -= secondsPlus_Change;
            SecondsUC.btnMinus.Click -= secondsMinus_Change;
        }
    }
}
