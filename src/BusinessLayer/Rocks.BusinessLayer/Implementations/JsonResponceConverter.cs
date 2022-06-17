using Newtonsoft.Json;
using Rocks.BusinessLayer.Abstractions;
using System.Text;

namespace Rocks.BusinessLayer.Implementations
{
    internal class JsonResponceConverter<T> : IResponceConverter<T>
    {
        public T Convert(byte[] data)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }
    }
}
