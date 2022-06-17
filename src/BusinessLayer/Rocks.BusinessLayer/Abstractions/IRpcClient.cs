using System;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Abstractions
{
    public interface IRpcClient<TRequest, TResponce> : IDisposable
    {
        TResponce Send(TRequest request);
        Task<TResponce> SendAsync(TRequest request);
    }
}