using Rocks.Shared.Data;
using System.Collections.Generic;

namespace Rocks.DataLayer.Abstractions
{
    public interface IVideoDeviceResolver
    {
        IEnumerable<VideoDeviceInfo> ResolveVideoDevices();
    }
}
