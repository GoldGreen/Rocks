using OpenCvSharp;
using Rocks.DataLayer.Abstractions;
using System.Collections.Generic;

namespace Rocks.DataLayer.Implementations
{
    internal class VideoReader : IVideoReader
    {
        public IEnumerable<Mat> LoadFrames(IVideoCaptureFactory videoFactory)
        {
            using var videoCapture = videoFactory.Create();
            while (true)
            {
                using Mat frame = new();
                if (!videoCapture.Read(frame))
                {
                    break;
                }
                yield return frame;
            }
        }
    }
}
