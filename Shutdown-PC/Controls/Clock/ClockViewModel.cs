using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Threading;

namespace Shutdown_PC.Controls.Clock
{

    public partial class ClockViewModel : ObservableObject
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();   //hodiny
        [ObservableProperty]
        private string _clockString;


        public ClockViewModel()
        {
            _timer.Tick += new EventHandler(timer_Click);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _timer.Start();
        }


        private void timer_Click(object sender, EventArgs e)
        {
            try
            {
                ClockString = DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception)
            {

            }
        }
    }
}
