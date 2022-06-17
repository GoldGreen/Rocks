using OpenCvSharp;
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
            container.RegisterScoped<IVideoService, VideoService>()
                     .RegisterScoped<IDetectionService, DetectionService>();

            container.RegisterSingleton(typeof(IRpcClient<,>), typeof(RpcClient<,>))
                     .RegisterScoped(typeof(IResponceConverter<>), typeof(JsonResponceConverter<>))
                     .RegisterScoped(typeof(IRequestConverter<>), typeof(JsonRequestConverter<>));

            container.RegisterScoped<IRequestConverter<Mat>, MatRequestConverter>();
        }
    }
}
