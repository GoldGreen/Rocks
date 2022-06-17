using Prism.Services.Dialogs;
using Rocks.Shared.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.PresentationLayer.Shared.Extensions
{
    public static class DialogExtensoins
    {
        public static string OpenCameraDialogName { get; } = "OpenCameraDialog";
        public static string DeviceParamaterName { get; } = "Device";

        public static string ConcatExtensions(this string name, params string[] extensions)
        {
            string ext = string.Join(';', extensions.Select(x => $"*.{x}"));
            return $"{name} ({ext})|{ext}";
        }

        public static Task<(bool ok, VideoDeviceInfo device)> OpenCameraDialog(this IDialogService dialogService)
        {
            TaskCompletionSource<(bool, VideoDeviceInfo)> task = new();

            dialogService.ShowDialog(OpenCameraDialogName, res =>
            {
                bool result = res.Result switch
                {
                    ButtonResult.OK => true,
                    _ => false
                };

                var videoDeviceInfo = result switch
                {
                    true => res.Parameters.GetValue<VideoDeviceInfo>(DeviceParamaterName),
                    _ => default
                };

                task.SetResult((result, videoDeviceInfo));
            });

            return task.Task;
        }
    }
}
