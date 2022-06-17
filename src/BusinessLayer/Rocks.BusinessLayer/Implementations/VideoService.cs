using OpenCvSharp;
using Rocks.BusinessLayer.Abstractions;
using Rocks.DataLayer.Abstractions;
using Rocks.Shared.Data;
using System.Collections.Generic;

namespace Rocks.BusinessLayer.Implementations
{
    internal class VideoService : IVideoService
    {
        public IVideoReader VideoReader { get; }
        public IVideoDeviceResolver VideoDeviceResolver { get; }

        public VideoService(IVideoReader videoReader, IVideoDeviceResolver videoDeviceResolver)
        {
            VideoReader = videoReader;
            VideoDeviceResolver = videoDeviceResolver;
        }

        public IEnumerable<Mat> LoadFramesFromCamera(int cameraId)
        {
            return VideoReader.LoadFrames(new VideoCaptureCameraFactory(cameraId));
        }

        public IEnumerable<Mat> LoadFramesFromFile(string fileName)
        {
            return VideoReader.LoadFrames(new VideoCaptureFileFactory(fileName));
        }

        public IEnumerable<VideoDeviceInfo> GetVideoDevices()
        {
            return VideoDeviceResolver.ResolveVideoDevices();
        }
    }
}
