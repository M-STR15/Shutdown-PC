using Ninject;
using ShutdownPC.ViewModels.Windows;

namespace ShutdownPC.Stores
{
	/// <summary>
	/// Store umožnující načítáí tříd z konteineru aplikace.
	/// </summary>
	public class ClasesStore
	{
		public ClasesStore(IKernel container)
		{
			_container = container;
		}

		private IKernel _container { get; set; }

		public CountdownPopupViewModel GetCountdownPopupViewModel() => _container.Get<CountdownPopupViewModel>();

		public InfoWindowViewModel GetInfoWindowViewModel() => _container.Get<InfoWindowViewModel>();

		public SettingWindowViewModel GetSettingWindowViewModel() => _container.Get<SettingWindowViewModel>();
	}
}