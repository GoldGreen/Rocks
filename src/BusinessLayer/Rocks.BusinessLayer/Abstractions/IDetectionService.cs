using OpenCvSharp;
using Rocks.Shared.Dto;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Abstractions
{
    public interface IDetectionService
    {
        Task<RocksDto> Detect(Mat mat);
    }
}