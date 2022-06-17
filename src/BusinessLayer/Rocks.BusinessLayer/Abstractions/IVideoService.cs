using Rocks.DataLayer.Abstractions;
using Rocks.Shared.Data;
using System.Collections.Generic;

namespace Rocks.BusinessLayer.Abstractions
{
    public interface IVideoService
    {
        IVideoFrames LoadFramesFromFile(string fileName);
        IVideoFrames LoadFramesFromCamera(int cameraId);
        IEnumerable<VideoDeviceInfo> GetVideoDevices();
    }
}
