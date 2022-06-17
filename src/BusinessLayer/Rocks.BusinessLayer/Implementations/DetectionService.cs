using OpenCvSharp;
using Rocks.BusinessLayer.Abstractions;
using Rocks.Shared.Dto;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Implementations
{
    internal class DetectionService : IDetectionService
    {
        public IRpcClient<Mat, RocksDto> RpcClient { get; }

        public DetectionService(IRpcClient<Mat, RocksDto> rpcClient)
        {
            RpcClient = rpcClient;
        }

        public Task<RocksDto> Detect(Mat mat)
        {
            return RpcClient.SendAsync(mat);
        }
    }
}
