using System.Configuration;
using System.Data;
using System.Windows;

namespace Shutdown_PC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            _ = Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            var mainViewModel = new MainViewModel();

            Current.MainWindow = mainWindow;
            Current.MainWindow.DataContext = mainViewModel;
            Current.MainWindow.Show();
        }
    }
}
