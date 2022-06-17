using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Rocks.BusinessLayer.Abstractions;
using Rocks.Detection.Models.Abstractions;

namespace Rocks.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoService VideoService { get; }
        [Reactive] public Mat CurrentFrame { get; set; }

        public DetectionModel(IVideoService videoService)
        {
            VideoService = videoService;
        }
    }
}
