using Prism.Ioc;
using Rocks.DataLayer.Abstractions;
using Rocks.DataLayer.Implementations;

namespace Rocks.DataLayer
{
    public static class DataLayerExtensions
    {
        public static void AddDataLayer(this IContainerRegistry container)
        {
            container.RegisterScoped<IVideoReader, VideoReader>();
            container.RegisterSingleton<IVideoDeviceResolver, VideoDeviceResolver>();
        }
    }
}
