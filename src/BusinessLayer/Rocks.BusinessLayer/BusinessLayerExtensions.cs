using OpenCvSharp;
using Prism.Ioc;
using Rabbit.Rpc;
using Rabbit.Rpc.Abstractions;
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
            container.AddRabbitRpc();

            container.RegisterScoped<IVideoService, VideoService>()
                     .RegisterScoped<IDetectionService, DetectionService>();

            container.RegisterScoped<IRequestConverter<Mat>, MatRequestConverter>();
        }
    }
}
