using Rocks.Detection.Models.Abstractions;

namespace Rocks.Detection.ViewModels
{
    internal class DetectionViewModel
    {
        public IDetectionModel Model { get; }
        
        public DetectionViewModel(IDetectionModel model)
        {
            Model = model;
        }
    }
}
