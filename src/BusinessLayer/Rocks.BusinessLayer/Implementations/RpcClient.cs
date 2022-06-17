using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rocks.BusinessLayer.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Implementations
{
    internal class RpcClient<TRequest, TResponce> : IRpcClient<TRequest, TResponce>
    {
        public IRequestConverter<TRequest> RequestConverter { get; }
        public IResponceConverter<TResponce> ResponceConverter { get; }
        public IQueueName<TRequest, TResponce> QueueName { get; }

        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<TResponce> respQueue = new();
        private readonly IBasicProperties props;

        public RpcClient(IRequestConverter<TRequest> requestConverter, IResponceConverter<TResponce> responceConverter, IQueueName<TRequest, TResponce> queueName)
        {
            RequestConverter = requestConverter;
            ResponceConverter = responceConverter;
            QueueName = queueName;

            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            string correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                if (ea.BasicProperties.CorrelationId != correlationId)
                {
                    return;
                }

                respQueue.Add(ResponceConverter.Convert(body));
            };

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
        }

        public TResponce Send(TRequest request)
        {
            channel.BasicPublish(
                exchange: "",
                routingKey: QueueName.QueueName,
                basicProperties: props,
                body: RequestConverter.Convert(request));

            return respQueue.Take();
        }
        public Task<TResponce> SendAsync(TRequest request)
        {
            return Task.Run(() => Send(request));
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
