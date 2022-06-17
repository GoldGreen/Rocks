using DirectShowLib;
using Rocks.DataLayer.Abstractions;
using Rocks.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.DataLayer.Implementations
{
    internal class VideoDeviceResolver : IVideoDeviceResolver
    {
        private readonly Lazy<IEnumerable<VideoDeviceInfo>> _videoDeviceInfos;

        public VideoDeviceResolver()
        {
            _videoDeviceInfos = new(() => LoadVideoDevicesInfo().ToList());
        }

        public IEnumerable<VideoDeviceInfo> ResolveVideoDevices()
        {
            return _videoDeviceInfos.Value;
        }

        private static IEnumerable<VideoDeviceInfo> LoadVideoDevicesInfo()
        {
            var captureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            for (int idx = 0; idx < captureDevices.Length; idx++)
            {
                yield return new(idx, captureDevices[idx].Name);
            }
        }
    }
}
