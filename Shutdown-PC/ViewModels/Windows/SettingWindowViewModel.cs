using ShutdownPC.Stores;

namespace ShutdownPC.ViewModels.Windows
{   /// <summary>
	/// ViewModel pro okno s nastavením aplikace.
	/// </summary>
	public partial class SettingWindowViewModel : BaseWindowViewModel
	{
		public SettingWindowViewModel(WindowStore windowStore) : base(windowStore)
		{
			Title = "Setting";
		}
	}
}