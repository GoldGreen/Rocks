namespace Rabbit.Rpc.Abstractions
{
    public interface IQueueName<TRequest, TResponce>
    {
        public string QueueName { get; }
    }
}
