using OpenCvSharp;
using Rabbit.Rpc.Abstractions;

namespace Rocks.BusinessLayer.Implementations
{
    internal class MatRequestConverter : IRequestConverter<Mat>
    {
        public byte[] Convert(Mat request)
        {
            return request.ToBytes(".jpg");
        }
    }
}
