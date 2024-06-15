using Ninject;
using Shutdown_PC.ViewModels.Windows;

namespace Shutdown_PC.Stores
{
    public class ClasesStore
    {
        private IKernel _container { get; set; }

        public ClasesStore(IKernel container)
        {
            _container = container;
        }

        public InfoWindowViewModel GetInfoWindowViewModel() => _container.Get<InfoWindowViewModel>();
        public SettingWindowViewModel GetSettingWindowViewModel() => _container.Get<SettingWindowViewModel>();
    }
}
