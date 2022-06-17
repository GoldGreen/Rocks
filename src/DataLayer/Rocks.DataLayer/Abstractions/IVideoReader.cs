using OpenCvSharp;
using System.Collections.Generic;

namespace Rocks.DataLayer.Abstractions
{
    public interface IVideoReader
    {
        IEnumerable<Mat> LoadFrames(IVideoCaptureFactory videoFactory);
    }
}
