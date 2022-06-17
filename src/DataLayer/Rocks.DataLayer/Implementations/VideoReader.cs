using Rocks.DataLayer.Abstractions;

namespace Rocks.DataLayer.Implementations
{
    internal class VideoReader : IVideoReader
    {
        public IVideoFrames LoadFrames(IVideoCaptureFactory videoFactory)
        {
            return new VideoFrames(videoFactory.Create());
        }
    }
}
