using CommunityToolkit.Mvvm.ComponentModel;
using Shutdown_PC.ViewModels.Windows;
using Shutdown_PC.Windows;
using System.Windows;

namespace Shutdown_PC.Stores
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
                var viewMode = _clasesStore.GetSettingWindowViewModel();
                var window = new SettingWindow();

                return (bool)showDialog<SettingWindowViewModel, SettingWindow>(viewMode, window, _mainWindow);
            }
            catch (Exception)
            {
            }

            return false;
        }

        private bool? showDialog<T, R>(T viewModel, R window, Window owner)
            where T : BaseWindowViewModel
            where R : Window
        {
            viewModel.Window = window;
            viewModel.Window.DataContext = viewModel;
            viewModel.Window.Owner = owner;
            viewModel.Window.HorizontalAlignment = HorizontalAlignment.Center;
            viewModel.Window.VerticalAlignment = VerticalAlignment.Center;

            return viewModel.Window.ShowDialog();
        }
    }
}