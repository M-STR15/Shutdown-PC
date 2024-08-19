using Ninject;
using Prism.Events;
using ShutdownPC.Services;
using ShutdownPC.Stores;
using ShutdownPC.ViewModels.Windows;
using System.IO;
using System.Reflection;
using System.Windows;

namespace ShutdownPC
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, IDisposable
	{
		private IKernel _container;

		public App()
		{}

		protected override void OnStartup(StartupEventArgs e)
		{
			configureContainer();
			registerEventSourceToWindowsLogers();

			InitializeComponent();
			base.OnStartup(e);

			Current.MainWindow = _container.Get<MainWindow>();
			Current.MainWindow.DataContext = _container.Get<MainViewModel>();
			Current.MainWindow.Show();
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
			_container.Bind<EventLogService>().To<EventLogService>().InSingletonScope();
			_container.Bind<PowerShellService>().To<PowerShellService>().InSingletonScope();
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
			_container.Bind<ClasesStore>().To<ClasesStore>().InSingletonScope()
				.WithConstructorArgument("container", _container);

			_container.Bind<WindowStore>().To<WindowStore>().InSingletonScope();
		}

		private void registerEventSourceToWindowsLogers()
		{
			var powerShellService = _container.Get<PowerShellService>();
			var path = Path.Combine(Assembly.GetExecutingAssembly().Location, "Scripts", "NewEventLog.ps1");
			powerShellService.RunForPath(path);
		}

		public void Dispose()
		{
			var logger = _container.Get<EventLogService>();
			if (logger != null)
				logger.Dispose();
		}
	}
}