using Ninject;
using Prism.Events;
using ShutdownPC.Services;
using ShutdownPC.Stores;
using ShutdownPC.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace ShutdownPC
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, IDisposable
	{
		private IKernel _container;
		private IEventLogService _log;
		private static Mutex s_mutex;

		public App()
		{ }

		[STAThread]
		protected override void OnStartup(StartupEventArgs e)
		{
			bool createdNew;
			s_mutex = new Mutex(true, "ShutdownPC", out createdNew);

			if (createdNew)
			{
				_log = new EventLogService();
				// Zachycení neošetřených výjimek na úrovni aplikačního vlákna
				AppDomain.CurrentDomain.UnhandledException += currentDomain_UnhandledException;
				// Zachycení neošetřených výjimek na úrovni dispatcheru (UI vlákno)
				DispatcherUnhandledException += app_DispatcherUnhandledException;

				configureContainer();

				InitializeComponent();
				base.OnStartup(e);

				Current.MainWindow = _container.Get<MainWindow>();
				Current.MainWindow.DataContext = _container.Get<MainViewModel>();
				Current.MainWindow.Show();
			}
			else
			{
				// Pokud již aplikace běží, informujeme uživatele nebo zavřeme tuto instanci.
				MessageBox.Show("Aplikace je již spuštěna.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
				Environment.Exit(0); // Ukončíme tuto instanci aplikace.
			}
		}

		private void configureContainer()
		{
			_container = new StandardKernel();

			_container.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

			configServices();

			configWindows();
			configVieModels();
			congfigStores();
		}

		private void configServices()
		{
			_container.Bind<IEventLogService>().To<EventLogService>().InSingletonScope();
			_container.Bind<IPowerShellService>().To<PowerShellService>().InSingletonScope();
		}

		private void configWindows()
		{
			_container.Bind<MainWindow>().To<MainWindow>().InSingletonScope();
		}

		private void configVieModels()
		{
			_container.Bind<MainViewModel>().To<MainViewModel>().InSingletonScope();
			_container.Bind<InfoWindowViewModel>().To<InfoWindowViewModel>().InSingletonScope();
			_container.Bind<SettingWindowViewModel>().To<SettingWindowViewModel>().InSingletonScope();
			_container.Bind<CountdownPopupViewModel>().To<CountdownPopupViewModel>().InSingletonScope();
		}

		private void congfigStores()
		{
			_container.Bind<IClasesStore>().To<ClasesStore>().InSingletonScope()
				.WithConstructorArgument("container", _container);

			_container.Bind<WindowStore>().To<WindowStore>().InSingletonScope();
		}

		public void Dispose()
		{
			var logger = _container.Get<IEventLogService>();
			if (logger != null)
				logger.Dispose();
		}

		private void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			_log.Error(Guid.Parse("c9c951ff-f176-4bcc-be5e-a87a9920c3e1"), $"Neočekávaná chyba (UI vlákno): {e.Exception.Message}");

			// Nastavení e.Handled na true zabrání aplikaci spadnout
			e.Handled = false;
		}

		private void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception ex)
				_log.Error(Guid.Parse("4fb577ad-9eba-4afa-9a89-368268989360"), $"Neočekávaná chyba (jiné vlákno): {ex.Message}");
			else
				_log.Error(Guid.Parse("fbc0e288-2f92-45e7-a8fc-2c5136c0dec0"), $"Došlo k neošetřené výjimce, která není typu Exception.");

		}

		private bool _isDragging;
		private Point _startPoint;

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				_isDragging = true;
				_startPoint = e.GetPosition((IInputElement)sender);
				(sender as UIElement)?.CaptureMouse();
			}
		}

		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				var window = Application.Current.MainWindow;
				if (window != null)
				{
					var currentPosition = e.GetPosition(window);
					window.Left += currentPosition.X - _startPoint.X;
					window.Top += currentPosition.Y - _startPoint.Y;
				}
			}
		}

		private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			_isDragging = false;
			(sender as UIElement)?.ReleaseMouseCapture();
		}
	}
}