using Ninject;
using ShutdownPC.ViewModels.Windows;

namespace ShutdownPC.Stores
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