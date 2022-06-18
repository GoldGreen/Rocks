using Rabbit.Rpc.Abstractions;

namespace Rabbit.Rpc.Implementations
{
    internal class ReflectionQueueName<TRequest, TResponce> : IQueueName<TRequest, TRequest>
    {
        public string QueueName { get; set; } = $"{typeof(TRequest).Name}_{typeof(TResponce).Name}";
    }
}
