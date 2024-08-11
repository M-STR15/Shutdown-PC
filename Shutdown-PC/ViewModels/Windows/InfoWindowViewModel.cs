using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Stores;

namespace ShutdownPC.ViewModels.Windows
{
    public partial class InfoWindowViewModel : BaseWindowViewModel
    {
        public InfoWindowViewModel(WindowStore windowStore) : base(windowStore)
        {
        }
    }
}