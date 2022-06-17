using Newtonsoft.Json;
using Rocks.BusinessLayer.Abstractions;
using System.Text;

namespace Rocks.BusinessLayer.Implementations
{
    internal class JsonRequestConverter<T> : IRequestConverter<T>
    {
        public byte[] Convert(T request)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
        }
    }
}
