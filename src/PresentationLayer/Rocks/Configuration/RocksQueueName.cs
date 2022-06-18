using OpenCvSharp;
using Rabbit.Rpc.Abstractions;
using Rocks.Shared.Dto;

namespace Rocks.Configuration
{
    internal class RocksQueueName : IQueueName<Mat, DetectioinResultDto>
    {
        public string QueueName => Settings.Default.RocksQueueName;
    }
}
