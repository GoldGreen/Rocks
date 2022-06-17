using OpenCvSharp;
using Rocks.DataLayer.Abstractions;

namespace Rocks.BusinessLayer.Implementations
{
    internal class VideoCaptureFileFactory : IVideoCaptureFactory
    {
        public string FileName { get; }

        public VideoCaptureFileFactory(string fileName)
        {
            FileName = fileName;
        }

        public VideoCapture Create()
        {
            return new(FileName);
        }
    }
}
