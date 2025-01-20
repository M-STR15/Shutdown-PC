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
			Thread.Sleep(800);	
			this.Show();

			Title = title;
			_minStartTime = minStartTime;
			_dateTimeStart = DateTime.Now;
			_timer = new DispatcherTimer();
			_timer.Interval = new TimeSpan(0, 0, 1);
			_timer.Tick += onControlTime_Tick;

			this.Set(0, "Start aplication...");
		}

		public EventHandler CloseWindowHandler { get; set; }

		/// <summary>
		/// Spustí časovač pro zavření okna.Ten následně zavře okno.
		/// </summary>
		public new void Close()
		{
			_timer.Start();
		}

		/// <summary>
		/// Nastaví hodnotu progresu a text poslední aktivity.
		/// </summary>
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

		/// <summary>
		/// Metoda, která se spustí při každém tiknutí časovače.
		/// Zkontroluje, zda uplynul minimální čas pro zobrazení okna.
		/// Pokud ano, zastaví časovač, zavře okno a vyvolá obslužnou rutinu zavření okna.
		/// </summary>
		private void onControlTime_Tick(object? sender, EventArgs e)
		{
			var duration = Convert.ToInt32((DateTime.Now - _dateTimeStart).TotalSeconds);
			if (duration > _minStartTime)
			{
				_timer.Stop();
				base.Close();
				onCloseWindowHandler();
			}
		}

		/// <summary>
		/// Vyvolá obslužnou rutinu zavření okna.
		/// </summary>
		private void onCloseWindowHandler()
		{
			CloseWindowHandler.Invoke(_timer, EventArgs.Empty);
		}
	}
}