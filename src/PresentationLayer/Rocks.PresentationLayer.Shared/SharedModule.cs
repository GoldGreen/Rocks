using Prism.Ioc;
using Prism.Modularity;
using Rocks.PresentationLayer.Shared.Extensions;
using Rocks.PresentationLayer.Shared.ViewModels;
using Rocks.PresentationLayer.Shared.Views;

namespace Rocks.PresentationLayer.Shared
{
    public class SharedModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<OpenCameraDialog, OpenCameraDialogViewModel>(DialogExtensoins.OpenCameraDialogName);
        }
    }
}
