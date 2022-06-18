using OpenCvSharp;
using Rocks.BusinessLayer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Abstractions
{
    public interface IDetectionService
    {
        Task<IEnumerable<Rock>> Detect(Mat mat);
    }
}