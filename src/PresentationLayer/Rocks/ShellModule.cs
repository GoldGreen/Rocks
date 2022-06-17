using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Rocks.ViewModels;
using Rocks.Views;

namespace Rocks
{
    internal class ShellModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<Shell, ShellViewModel>();
        }
    }
}
