using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Models;
using ShutdownPC.Stores;

namespace ShutdownPC.ViewModels.Windows
{
	/// <summary>
	/// ViewModel pro okno s informacemi o aplikaci.
	/// </summary>
	public partial class InfoWindowViewModel : BaseWindowViewModel
	{
		[ObservableProperty]
		private string _version;

		public InfoWindowViewModel(WindowStore windowStore) : base(windowStore)
		{
			Title = "Info";
			Version = BuildInfo.VersionStr;
		}
	}
}