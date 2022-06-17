using Microsoft.Win32;
using OpenCvSharp.WpfExtensions;
using Prism.Services.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Rocks.Detection.Models.Abstractions;
using Rocks.PresentationLayer.Shared.Extensions;
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace Rocks.Detection.ViewModels
{
    internal class DetectionViewModel : ReactiveObject
    {
        public IDetectionModel Model { get; }
        public IDialogService DialogService { get; }

        [Reactive] public ImageSource CurrentFrame { get; set; }

        public ICommand OpenCameraCommand { get; }
        public ICommand OpenFileCommand { get; }

        public DetectionViewModel(IDetectionModel model, IDialogService dialogService)
        {
            Model = model;
            DialogService = dialogService;
            Model.WhenAnyValue(x => x.CurrentFrame)
                 .WhereNotNull()
                 .Select(BitmapSourceConverter.ToBitmapSource)
                 .Subscribe(x => CurrentFrame = x);

            OpenFileCommand = ReactiveCommand.Create(() =>
            {
                OpenFileDialog openFileDialog = new();

                string videoExtensions = "Видео".ConcatExtensions("AVI", "MP4", "MPEG", "MOV");
                string imageExtensions = "Изображения".ConcatExtensions("BMP", "JPEG", "JPG", "PNG");

                openFileDialog.Filter = string.Join('|', videoExtensions, imageExtensions);

                if (openFileDialog.ShowDialog() == true)
                {
                }
            });

            OpenCameraCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var (ok, device) = await DialogService.OpenCameraDialog();
                if (ok)
                {
                }
            });
        }
    }
}
