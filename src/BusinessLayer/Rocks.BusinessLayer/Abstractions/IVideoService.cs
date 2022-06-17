using OpenCvSharp;
using Rocks.Shared.Data;
using System.Collections.Generic;

namespace Rocks.BusinessLayer.Abstractions
{
    public interface IVideoService
    {
        IEnumerable<Mat> LoadFramesFromFile(string fileName);
        IEnumerable<Mat> LoadFramesFromCamera(int cameraId);
        IEnumerable<VideoDeviceInfo> GetVideoDevices();
    }
}
