using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.ViewModels.Windows;
using ShutdownPC.Windows;
using System.Windows;
using System.Windows.Media.Effects;

namespace ShutdownPC.Stores
{
    public partial class WindowStore : ObservableObject
    {
        private readonly ClasesStore _clasesStore;
        private readonly MainWindow _mainWindow;

        [ObservableProperty]
        private TimeSpan _setTimeValueProperty;

        public WindowStore(ClasesStore clasesStore, MainWindow mainWindow)
        {
            _clasesStore = clasesStore;
            _mainWindow = mainWindow;
        }

        public bool ShowSettigWindow()
        {
            try
            {
                var viewModel = _clasesStore.GetSettingWindowViewModel();
                var window = new SettingWindow();

                return (bool)showDialog<SettingWindowViewModel, SettingWindow>(viewModel, window, _mainWindow);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ShowInfoWindow()
        {
            try
            {
                var viewModel = _clasesStore.GetInfoWindowViewModel();
                var window = new InfoWindow();
                return (bool)showDialog<InfoWindowViewModel, InfoWindow>(viewModel, window, _mainWindow);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ShowCountdownPopupWindow()
        {
            try
            {
                var viewModel = _clasesStore.GetCountdownPopupViewModel();
                var window = new CountdownPopupWindow();
                return (bool)showDialog<CountdownPopupViewModel, CountdownPopupWindow>(viewModel, window, _mainWindow);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CloseWindow<T>(T viewModel)
             where T : BaseWindowViewModel
        {
            try
            {
                closeWindow(viewModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool? showDialog<T, R>(T viewModel, R window, Window owner)
            where T : BaseWindowViewModel
            where R : Window
        {

            BlurEffect objBlur = new BlurEffect();
            objBlur.Radius = 4;

            viewModel.Window = window;
            viewModel.Window.DataContext = viewModel;
            viewModel.Window.Owner = owner;
            viewModel.Window.Owner.Effect = objBlur;
            viewModel.Window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            viewModel.Window.HorizontalAlignment = HorizontalAlignment.Center;
            viewModel.Window.VerticalAlignment = VerticalAlignment.Center;
            viewModel.Window.Topmost = true;

            return viewModel.Window.ShowDialog();
        }

        private void closeWindow<T>(T viewModel)
              where T : BaseWindowViewModel
        {
            // Zavřete okno (implementace závisí na frameworku, např. Window.Close())
            if (viewModel.Window is Window)
            {
                BlurEffect objBlur = new BlurEffect();
                objBlur.Radius = 0;
                viewModel.Window.Owner.Effect = objBlur;
            }

            viewModel.Window?.Close();
        }
    }
}