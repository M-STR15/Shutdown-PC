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

        public ClockControl()
        {
            InitializeComponent();

            _timer.Tick += new EventHandler(timer_Click);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _timer.Start();
        }

        private void timer_Click(object sender, EventArgs e)
        {
            try
            {
                var time = DateTime.Now;
                lblHours.Content = time.ToString("HH");
                lblMinutes.Content = time.ToString("mm");
                lblSeconds.Content = time.ToString("ss");

                lblDate.Content = DateTime.Now.ToShortDateString();
            }
            catch (Exception)
            {
            }
        }
    }
}