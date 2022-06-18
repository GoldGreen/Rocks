using Newtonsoft.Json;
using Rabbit.Rpc.Abstractions;
using System.Text;

namespace Rabbit.Rpc.Implementations
{
    internal class JsonRequestConverter<T> : IRequestConverter<T>
    {
        public byte[] Convert(T request)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
        }
    }
}
