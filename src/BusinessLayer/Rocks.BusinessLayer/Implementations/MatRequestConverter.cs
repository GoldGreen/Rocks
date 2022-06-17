using OpenCvSharp;
using Rocks.BusinessLayer.Abstractions;

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
