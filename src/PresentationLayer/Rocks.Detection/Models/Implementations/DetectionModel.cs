using ReactiveUI;
using Rocks.BusinessLayer.Abstractions;
using Rocks.Detection.Models.Abstractions;

namespace Rocks.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoService VideoService { get; }

        public DetectionModel(IVideoService videoService)
        {
            VideoService = videoService;
        }
    }
}
