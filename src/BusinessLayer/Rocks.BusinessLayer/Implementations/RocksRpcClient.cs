using Newtonsoft.Json;
using OpenCvSharp;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.BusinessLayer.Implementations
{
    public class RocksRpcClient : RpcClientBase
    {
        public RocksRpcClient()
            : base("rocks")
        {
        }

        public async Task<List<List<List<double>>>> Call(Mat mat)
        {
            var res = await Task.Run(() => Call(mat.ToBytes(".jpg")));

            string response = Encoding.UTF8.GetString(res);

            return JsonConvert.DeserializeObject<List<List<List<double>>>>(response);
        }
    }
}
