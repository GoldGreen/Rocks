using Prism.Ioc;
using Rabbit.Rpc.Abstractions;
using Rabbit.Rpc.Implementations;

namespace Rabbit.Rpc
{
    public static class RabbitRpcExtensions
    {
        public static void AddRabbitRpc(this IContainerRegistry container)
        {
            container.RegisterSingleton(typeof(IRpcClient<,>), typeof(RpcClient<,>))
                     .RegisterScoped(typeof(IResponceConverter<>), typeof(JsonResponceConverter<>))
                     .RegisterScoped(typeof(IRequestConverter<>), typeof(JsonRequestConverter<>))
                     .RegisterScoped(typeof(IQueueName<,>), typeof(ReflectionQueueName<,>));
        }
    }
}
