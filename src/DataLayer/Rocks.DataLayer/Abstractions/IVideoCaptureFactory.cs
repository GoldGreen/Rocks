using OpenCvSharp;

namespace Rocks.DataLayer.Abstractions
{
    public interface IVideoCaptureFactory
    {
        VideoCapture Create();
    }
}
