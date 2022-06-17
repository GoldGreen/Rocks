using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Rocks.Detection.Models.Abstractions;
using Rocks.Detection.Models.Implementations;
using Rocks.Detection.ViewModels;
using Rocks.PresentationLayer.Shared.Views;

namespace Rocks.Detection
{
    public class DetectionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>()
                             .RegisterViewWithRegion<Views.Detection>(GlobalRegions.Detection);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IDetectionModel, DetectionModel>();

            ViewModelLocationProvider.Register<Views.Detection, DetectionViewModel>();
        }
    }
}
