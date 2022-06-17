using OpenCvSharp;
using ReactiveUI;
using System.Threading;
using System.Threading.Tasks;

namespace Rocks.Detection.Models.Abstractions
{
    internal interface IDetectionModel : IReactiveObject
    {
        Mat CurrentFrame { get; set; }
        bool ReadingFrames { get; set; }
        bool Paused { get; set; }

        Task StartVideoFromCamera(int cameraId, CancellationTokenSource cancellationTokenSource);
        Task StartVideoFromFile(string fileName, CancellationTokenSource cancellationTokenSource);
        void StopVideo(CancellationTokenSource cancellationTokenSource);
    }
}
