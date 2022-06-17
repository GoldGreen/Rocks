namespace Rocks.BusinessLayer.Abstractions
{
    public interface IQueueName<TRequest, TResponce>
    {
        public string QueueName { get; }
    }
}
