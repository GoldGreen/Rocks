using Microsoft.Win32;
using OpenCvSharp.WpfExtensions;
using Prism.Services.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Rocks.Detection.Models.Abstractions;
using Rocks.PresentationLayer.Shared.Extensions;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;

namespace Rocks.Detection.ViewModels
{
    internal class DetectionViewModel : ReactiveObject
    {
        public IDetectionModel Model { get; }
        public IDialogService DialogService { get; }

        [Reactive] public ImageSource CurrentFrame { get; set; }
        [Reactive] public CancellationTokenSource CancellationTokenSource { get; set; }

        public ICommand OpenCameraCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand StopVideoCommand { get; }
        public ICommand RevercePauseCommand { get; }

        public DetectionViewModel(IDetectionModel model, IDialogService dialogService)
        {
            Model = model;
            DialogService = dialogService;
            Model.WhenAnyValue(x => x.CurrentFrame)
                 .WhereNotNull()
                 .Select(BitmapSourceConverter.ToBitmapSource)
                 .Subscribe(x => CurrentFrame = x);

            var canStartVideo = Model.WhenAnyValue(x => x.ReadingFrames).Select(x => !x);

            OpenFileCommand = ReactiveCommand.Create(async () =>
            {
                OpenFileDialog openFileDialog = new();

                string videoExtensions = "Видео".ConcatExtensions("AVI", "MP4", "MPEG", "MOV");
                string imageExtensions = "Изображения".ConcatExtensions("BMP", "JPEG", "JPG", "PNG");

                openFileDialog.Filter = string.Join('|', videoExtensions, imageExtensions);

                if (openFileDialog.ShowDialog() == true)
                {
                    CancellationTokenSource = new();
                    await Model.StartVideoFromFile(openFileDialog.FileName, CancellationTokenSource);
                }
            }, canStartVideo);

            OpenCameraCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var (ok, device) = await DialogService.OpenCameraDialog();

                if (ok)
                {
                    CancellationTokenSource = new();
                    await Model.StartVideoFromCamera(device.Id, CancellationTokenSource);
                }
            }, canStartVideo);

            StopVideoCommand = ReactiveCommand.Create(() =>
            {
                Model.StopVideo(CancellationTokenSource);
                CancellationTokenSource = default;
            }, this.WhenAnyValue(x => x.CancellationTokenSource,
                                 x => x.Model.ReadingFrames,
                                 (ct, rf) => ct != null && rf));

            RevercePauseCommand = ReactiveCommand.Create(() =>
            Model.Paused = !Model.Paused,
            Model.WhenAnyValue(x => x.CurrentFrame,
                               x => x.ReadingFrames,
                               (ct, rf) => ct != null && rf));
        }
    }
}
