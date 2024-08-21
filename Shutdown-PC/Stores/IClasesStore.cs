using ShutdownPC.ViewModels.Windows;

namespace ShutdownPC.Stores
{
	public interface IClasesStore
	{
		CountdownPopupViewModel GetCountdownPopupViewModel();
		InfoWindowViewModel GetInfoWindowViewModel();
		SettingWindowViewModel GetSettingWindowViewModel();
	}
}