using ShutdownPC.Stores;
using System.Windows.Input;

namespace ShutdownPC.ViewModels.Windows
{
    public partial class SettingWindowViewModel : BaseWindowViewModel
    {
        public SettingWindowViewModel(WindowStore windowStore):base(windowStore)
        {
            Title = "Setting";
        }
    }
}