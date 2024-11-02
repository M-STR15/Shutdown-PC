using System.Windows;
using System.Windows.Threading;

namespace ShutdownPC.SplashScreen
{
	/// <summary>
	/// Hlavní zobrazovací okno pro SplashScreen.
	/// </summary>
	public partial class StartWindow : Window
	{
		private DateTime _dateTimeStart;
		private int _minStartTime;
		private DispatcherTimer _timer;

		public StartWindow(string title, int minStartTime = 7)
		{
			InitializeComponent();

			Title = title;
			_minStartTime = minStartTime;
			_dateTimeStart = DateTime.Now;
			_timer = new DispatcherTimer();
			_timer.Interval = new TimeSpan(0, 0, 1);
			_timer.Tick += _controlTime_Tick;

			this.Show();
			this.Set(0, "Start aplication...");
		}

		public EventHandler CloseWindowHandler { get; set; }

		/// <summary>
		/// Vypre uvoldní obrazovku, ale až po uplinutí minimálního času
		/// </summary>
		public async new void Close()
		{
			_timer.Start();
		}

		public void Set(int progress, string textLastActivit = "")
		{
			if (progress < 10)
				progressValue.Text = "00" + progress.ToString();
			else if (progress < 100)
				progressValue.Text = "0" + progress.ToString();
			else if (progress == 100)
				progressValue.Text = progress.ToString();

			txtInfo.Text = textLastActivit;
		}

		private void _controlTime_Tick(object? sender, EventArgs e)
		{
			var duration = Convert.ToInt32((DateTime.Now - _dateTimeStart).TotalSeconds);
			if (duration > _minStartTime)
			{
				_timer.Stop();
				base.Close();
				onCloseWindowHandler();
			}
		}

		private void onCloseWindowHandler()
		{
			CloseWindowHandler.Invoke(_timer, EventArgs.Empty);
		}
	}
}