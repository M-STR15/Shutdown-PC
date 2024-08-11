using Ninject;
using ShutdownPC.ViewModels.Windows;

namespace ShutdownPC.Stores
{
    public class ClasesStore
    {
        public ClasesStore(IKernel container)
        {
            _container = container;
        }

        private IKernel _container { get; set; }
        public InfoWindowViewModel GetInfoWindowViewModel() => _container.Get<InfoWindowViewModel>();

        public SettingWindowViewModel GetSettingWindowViewModel() => _container.Get<SettingWindowViewModel>();

        public CountdownPopupViewModel GetCountdownPopupViewModel() => _container.Get<CountdownPopupViewModel>();
    }
}