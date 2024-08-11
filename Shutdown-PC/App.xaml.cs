using Ninject;
using ShutdownPC.Stores;
using ShutdownPC.ViewModels.Windows;
using System.Windows;

namespace ShutdownPC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        public App()
        {
            InitializeComponent();
            _ = Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            configureContainer();

            Current.MainWindow = _container.Get<MainWindow>();
            Current.MainWindow.DataContext = _container.Get<MainViewModel>();
            Current.MainWindow.Show();
        }

        private void configureContainer()
        {
            _container = new StandardKernel();

            configWindows();
            configVieModels();
            congfigStores();
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
    }
}