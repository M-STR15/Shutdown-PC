using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Stores;
using System.Windows;
using System.Windows.Input;

namespace ShutdownPC.ViewModels.Windows
{
    [ObservableObject]
    public partial class BaseWindowViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _title;
        public Window Window { get; set; }

        protected WindowStore _windowStore;
        public ICommand CloseWindowCommand { get; private set; }
        public BaseWindowViewModel(WindowStore windowStore)
        {
            _windowStore = windowStore;
            CloseWindowCommand = new Helpers.RelayCommand(cmd_CloseWindow);
        }

        private void cmd_CloseWindow(object parameter)
        {
            _windowStore.CloseWindow<BaseWindowViewModel>(this);
        }
    }
}