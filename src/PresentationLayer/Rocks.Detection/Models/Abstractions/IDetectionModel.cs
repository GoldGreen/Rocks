using OpenCvSharp;
using ReactiveUI;

namespace Rocks.Detection.Models.Abstractions
{
    internal interface IDetectionModel : IReactiveObject
    {
        Mat CurrentFrame { get; set; }
    }
}
