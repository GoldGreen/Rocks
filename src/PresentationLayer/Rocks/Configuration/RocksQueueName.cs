using OpenCvSharp;
using Rocks.BusinessLayer.Abstractions;
using Rocks.Shared.Dto;

namespace Rocks.Configuration
{
    internal class RocksQueueName : IQueueName<Mat, RocksDto>
    {
        public string QueueName => Settings.Default.RocksQueueName;
    }
}
