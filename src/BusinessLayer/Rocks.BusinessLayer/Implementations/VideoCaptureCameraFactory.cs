using OpenCvSharp;
using Rocks.DataLayer.Abstractions;

namespace Rocks.BusinessLayer.Implementations
{
    internal class VideoCaptureCameraFactory : IVideoCaptureFactory
    {
        public int CameraId { get; }

        public VideoCaptureCameraFactory(int cameraId)
        {
            CameraId = cameraId;
        }

        public VideoCapture Create()
        {
            return new(CameraId);
        }
    }
}
