using CommunityToolkit.Mvvm.ComponentModel;

namespace ShutdownPC.ViewModels.Windows
{
    [ObservableObject]
    public partial class InfoWindowViewModel : BaseWindowViewModel
    {
        [ObservableProperty]
        private string _hours;

        [ObservableProperty]
        private string _minutes;

        [ObservableProperty]
        private string _seconds;

    }
}