using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ShutdownPC.Controls
{
    /// <summary>
    /// Interaction logic for ClockView.xaml
    /// </summary>
    public partial class ClockControl : UserControl
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();   //hodiny

        public static readonly DependencyProperty ClockTimeProperty =
           DependencyProperty.Register
           (nameof(ClockTime),
            typeof(DateTime),
            typeof(ClockControl));

        public DateTime ClockTime
        {
            get => (DateTime)GetValue(ClockTimeProperty);
            set => SetValue(ClockTimeProperty, value);
        }
        public ClockControl()
        {
            InitializeComponent();

            _timer.Tick += new EventHandler(timer_Click);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer.Start();
        }

        private void timer_Click(object sender, EventArgs e)
        {
            try
            {
                ClockTime = DateTime.Now;
                lblHours.Content = ClockTime.ToString("HH");
                lblMinutes.Content = ClockTime.ToString("mm");
                lblSeconds.Content = ClockTime.ToString("ss");

                lblDate.Content = DateTime.Now.ToShortDateString();
            }
            catch (Exception)
            {
            }
        }
    }
}