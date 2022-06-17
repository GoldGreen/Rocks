using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;

namespace Rocks.BusinessLayer.Implementations
{
    public class RpcClientBase : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<byte[]> respQueue = new();
        private readonly IBasicProperties props;
        private readonly string routingKey;

        public RpcClientBase(string routingKey)
        {
            this.routingKey = routingKey;

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
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(body);
                }
            };

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
        }

        public byte[] Call(byte[] messageBytes)
        {
            channel.BasicPublish(
                exchange: "",
                routingKey: routingKey,
                basicProperties: props,
                body: messageBytes);

            return respQueue.Take();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
