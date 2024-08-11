using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Stores;

namespace ShutdownPC.ViewModels.Windows
{
    public partial class CountdownPopupViewModel : BaseWindowViewModel
    {
        [ObservableProperty]
        private string _hours;

        [ObservableProperty]
        private string _minutes;

        [ObservableProperty]
        private string _seconds;

        public CountdownPopupViewModel(WindowStore windowStore) : base(windowStore)
        {
        }
    }
}
