using Newtonsoft.Json;
using Rabbit.Rpc.Abstractions;
using System.Text;

namespace Rabbit.Rpc.Implementations
{
    internal class JsonResponceConverter<T> : IResponceConverter<T>
    {
        public T Convert(byte[] data)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }
    }
}
