using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Models.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ShutdownPC.Controls
{
    /// <summary>
    /// Interaction logic for SetTimerControl.xaml
    /// </summary>
    [ObservableObject]
    public partial class SetTimerControl : UserControl, IDisposable
    {
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
             new FrameworkPropertyMetadata(eStatus.Run, new PropertyChangedCallback(onStatusPropertyChanged)));

        private int _endAfterSeconds;

        public eStatus Status
        {
            get => (eStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        private DispatcherTimer t_CountdownTimer;

        private DateTime _endDateTime;

        public SetTimerControl()
        {
            InitializeComponent();

            HoursUC.btnPlus.Click += hoursPlus_Change;
            HoursUC.btnMinus.Click += hoursMinus_Change;

            MinutesUC.btnPlus.Click += minutesPlus_Change;
            MinutesUC.btnMinus.Click += minutesMinus_Change;

            SecondsUC.btnPlus.Click += secondsPlus_Change;
            SecondsUC.btnMinus.Click += secondsMinus_Change;

            _endDateTime = DateTime.Now;

            t_CountdownTimer = new DispatcherTimer();
            t_CountdownTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            t_CountdownTimer.Tick += new EventHandler(onCountdown_Tick);
        }

        private void onCountdown_Tick(object sender, EventArgs args)
        {
            if (SetTimeValue <= DateTime.Now)
            {
                t_CountdownTimer.Stop();
                Status = eStatus.Stop;
                MessageBox.Show("test timeru");
            }
            else
            {
                _endAfterSeconds = (int)(SetTimeValue - DateTime.Now).TotalSeconds;
                setLabelTimer();
            }
        }

        private void changeStatus()
        {
            if (Status == eStatus.Run)
                t_CountdownTimer.Start();
            else
                t_CountdownTimer.Stop();
        }

        private void hoursPlus_Change(object sender, EventArgs args)
        {
            setTimeValue(+3600);
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setAllButtons();
            setLabelTimer();
        }

        private void hoursMinus_Change(object sender, EventArgs args)
        {
            setTimeValue(-3600);
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setAllButtons();
            setLabelTimer();
        }

        private void minutesPlus_Change(object sender, EventArgs args)
        {
            setTimeValue(+60);
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setAllButtons();
            setLabelTimer();
        }

        private void minutesMinus_Change(object sender, EventArgs args)
        {
            setTimeValue(-60);
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setAllButtons();
            setLabelTimer();
        }

        private void secondsPlus_Change(object sender, EventArgs args)
        {
            setTimeValue(+1);
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setAllButtons();
            setLabelTimer();
        }

        private void secondsMinus_Change(object sender, EventArgs args)
        {
            setTimeValue(-1);
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setAllButtons();
            setLabelTimer();
        }

        private void setAllButtons()
        {
            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setMinusButton(SecondsUC, time.Seconds, time.Minutes);
            setMinusButton(MinutesUC, time.Minutes, time.Hours);
            setMinusButton(HoursUC, time.Hours);
        }
        private void setMinusButton(NumericControl numericControl, int timeValue, int previousTimeValue = 0)
        {
            if (TypeModification == eTypeModification.AfterTime)
            {
                var isEnable = (timeValue > 0) || previousTimeValue > 0;
                numericControl.btnMinus.IsEnabled = isEnable;
            }
            else
            {
                numericControl.btnMinus.IsEnabled = true;
            }
        }

        private void setTimeValue(int addSeconds)
        {
            _endAfterSeconds += addSeconds;
            SetTimeValue = _endDateTime.AddSeconds(_endAfterSeconds);
        }

        private void setLabelTimer()
        {
            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    var endAdterSeconds = DateTime.Now.AddSeconds((SetTimeValue - DateTime.Now).TotalSeconds);
                    HoursUC.TimeValue = endAdterSeconds.Hour;
                    MinutesUC.TimeValue = endAdterSeconds.Minute;
                    SecondsUC.TimeValue = endAdterSeconds.Second;

                    lblDate.Content = endAdterSeconds.ToShortDateString();
                    lblNegativTime.Visibility = Visibility.Hidden;
                    break;

                case eTypeModification.AfterTime:
                    var time = TimeSpan.FromSeconds(_endAfterSeconds);
                    HoursUC.TimeValue = (int)time.TotalHours;
                    MinutesUC.TimeValue = time.Minutes;
                    SecondsUC.TimeValue = time.Seconds;

                    lblDate.Content = "";

                    lblNegativTime.Visibility = time.TotalSeconds < 0 ? Visibility.Visible : Visibility.Hidden;
                    break;
            }

        }

        public DateTime SetTimeValue
        {
            get => (DateTime)GetValue(SetTimeValueProperty);
            set => SetValue(SetTimeValueProperty, value);
        }

        public eTypeModification TypeModification
        {
            get => (eTypeModification)GetValue(TypeModificationProperty);
            set => SetValue(TypeModificationProperty, value);
        }

        private static void onTypeModificationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as SetTimerControl;
            uc.onTypeModificationPropertyChanged(e);
        }

        private void onTypeModificationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            setLabelTimer();
            setAllButtons();

            switch (TypeModification)
            {
                case eTypeModification.InTime:
                    _endDateTime = DateTime.Now;
                    break;
            }
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

            var time = TimeSpan.FromSeconds(_endAfterSeconds);
            setMinusButton(HoursUC, time.Hours);
            setMinusButton(MinutesUC, time.Minutes);
            setMinusButton(SecondsUC, time.Seconds);

            changeStatus();
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