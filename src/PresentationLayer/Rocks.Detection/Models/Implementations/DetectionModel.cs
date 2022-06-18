using OpenCvSharp;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Rocks.BusinessLayer.Abstractions;
using Rocks.BusinessLayer.Data;
using Rocks.DataLayer.Abstractions;
using Rocks.Detection.Models.Abstractions;
using System;
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
                                var rocks = await DetectionService.Detect(frame);

                                foreach (var rock in rocks)
                                {
                                    DrawRock(frame, rock);
                                }

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

        private static void DrawRock(Mat frame, Rock rock, double alpha = 0.1)
        {
            var clone = frame.EmptyClone();

            Cv2.DrawContours(clone, rock.Polygones, -1, Scalar.Red, -1);

            Cv2.AddWeighted(clone, alpha, frame, 1, 0, frame);
            //Cv2.Rectangle(frame, rock.Rectangle, Scalar.DarkRed, 4);
        }
    }
}
