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
		public static readonly DependencyProperty ClockTimeProperty =
		   DependencyProperty.Register
		   (nameof(ClockTime),
			typeof(DateTime),
			typeof(ClockControl));

		private readonly DispatcherTimer _timer = new DispatcherTimer();   //hodiny

		public ClockControl()
		{
			InitializeComponent();

			_timer.Tick += new EventHandler(timer_Click);
			_timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
			_timer.Start();
		}

		public DateTime ClockTime
		{
			get => (DateTime)GetValue(ClockTimeProperty);
			set => SetValue(ClockTimeProperty, value);
		}

		/// <summary>
		/// Metoda, která se spustí při každém tiknutí časovače.
		/// Aktualizuje čas a datum na uživatelském rozhraní.
		/// </summary>
		private void timer_Click(object sender, EventArgs e)
		{
			try
			{
				var currentTime = DateTime.Now;
				ClockTime = currentTime;
				lblHours.Content = ClockTime.ToString("HH");
				lblMinutes.Content = ClockTime.ToString("mm");
				lblSeconds.Content = ClockTime.ToString("ss");

				lblDate.Content = currentTime.ToShortDateString();
			}
			catch (Exception)
			{
			}
		}
	}
}