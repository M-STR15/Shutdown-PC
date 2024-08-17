using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Stores;

namespace ShutdownPC.ViewModels.Windows
{
    public partial class InfoWindowViewModel : BaseWindowViewModel
    {
        [ObservableProperty]
        private string _version;

        public InfoWindowViewModel(WindowStore windowStore) : base(windowStore)
        {
            Title = "Info";
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}