using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Rocks.BusinessLayer.Abstractions;
using Rocks.DataLayer.Abstractions;
using Rocks.Detection.Models.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rocks.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoService VideoService { get; }
        public IDetectionService DetectionService { get; }
        [Reactive] public Mat CurrentFrame { get; set; }
        [Reactive] public bool ReadingFrames { get; set; }
        [Reactive] public bool Paused { get; set; }

        public DetectionModel(IVideoService videoService, IDetectionService detectionService)
        {
            VideoService = videoService;
            DetectionService = detectionService;
        }

        public Task StartVideoFromFile(string fileName, CancellationTokenSource cancellationTokenSource)
        {
            return StartVideo(() => VideoService.LoadFramesFromFile(fileName), cancellationTokenSource);
        }

        public Task StartVideoFromCamera(int cameraId, CancellationTokenSource cancellationTokenSource)
        {
            return StartVideo(() => VideoService.LoadFramesFromCamera(cameraId), cancellationTokenSource);
        }

        private async Task StartVideo(Func<IVideoFrames> framesFactory, CancellationTokenSource cancellationTokenSource)
        {
            if (ReadingFrames)
            {
                return;
            }

            ReadingFrames = true;
            await Task.Run
            (
                async () =>
                {
                    using var frames = framesFactory();
                    foreach (var frame in frames.GetFrames())
                    {
                        await System.Windows.Application.Current.Dispatcher.Invoke
                        (
                            async () =>
                            {
                                var res = await DetectionService.Detect(frame);

                                Cv2.DrawContours(frame, res.SelectMany(x => x).Select(polygon =>
                                {
                                    var x = polygon.Where((x, i) => i % 2 == 0);
                                    var y = polygon.Where((x, i) => i % 2 != 0);
                                    return x.Zip(y, (x, y) => new Point(x, y)).ToArray();
                                }).ToArray(), -1, Scalar.Red, 5);

                                CurrentFrame = frame;

                                while (Paused)
                                {
                                    await Task.Delay(100);
                                }
                            }
                        );

                        if (cancellationTokenSource.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            );
            ReadingFrames = false;
        }

        public void StopVideo(CancellationTokenSource cancellationTokenSource)
        {
            if (ReadingFrames)
            {
                cancellationTokenSource.Cancel();
                ReadingFrames = false;
            }
        }
    }
}
