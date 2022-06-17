using OpenCvSharp;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Rocks.BusinessLayer.Abstractions;
using Rocks.Configuration;
using Rocks.Shared.Dto;
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
            containerRegistry.RegisterScoped<IQueueName<Mat, RocksDto>, RocksQueueName>();
            ViewModelLocationProvider.Register<Shell, ShellViewModel>();
        }
    }
}
