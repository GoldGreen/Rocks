using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Rocks.BusinessLayer;
using Rocks.Detection;
using Rocks.PresentationLayer.Shared;
using Rocks.Views;
using System.Windows;

namespace Rocks
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.AddBusinessLayer();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ShellModule>();
            moduleCatalog.AddModule<SharedModule>();

            moduleCatalog.AddModule<DetectionModule>();

            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
