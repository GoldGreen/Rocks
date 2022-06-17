using Prism.Ioc;
using Rocks.BusinessLayer.Abstractions;
using Rocks.BusinessLayer.Implementations;
using Rocks.DataLayer;

namespace Rocks.BusinessLayer
{
    public static class BusinessLayerExtensions
    {
        public static void AddBusinessLayer(this IContainerRegistry container)
        {
            container.AddDataLayer();
            container.RegisterScoped<IVideoService, VideoService>();
        }
    }
}
