namespace Rocks.DataLayer.Abstractions
{
    public interface IVideoReader
    {
        IVideoFrames LoadFrames(IVideoCaptureFactory videoFactory);
    }
}
