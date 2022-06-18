using System;
using System.Threading.Tasks;

namespace Rabbit.Rpc.Abstractions
{
    public interface IRpcClient<TRequest, TResponce> : IDisposable
    {
        TResponce Send(TRequest request);
        Task<TResponce> SendAsync(TRequest request);
    }
}