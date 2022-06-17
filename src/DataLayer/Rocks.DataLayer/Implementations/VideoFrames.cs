using OpenCvSharp;
using Rocks.DataLayer.Abstractions;
using System.Collections.Generic;

namespace Rocks.DataLayer.Implementations
{
    internal class VideoFrames : IVideoFrames
    {
        public VideoCapture VideoCapture { get; }

        public VideoFrames(VideoCapture videoCapture)
        {
            VideoCapture = videoCapture;
        }

        public IEnumerable<Mat> GetFrames()
        {
            while (true)
            {
                using Mat frame = new();
                if (!VideoCapture.Read(frame))
                {
                    break;
                }
                yield return frame;
            }
        }

        public void Dispose()
        {
            VideoCapture.Dispose();
        }
    }
}
