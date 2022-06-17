namespace Rocks.BusinessLayer.Abstractions
{
    public interface IResponceConverter<T>
    {
        T Convert(byte[] data);
    }
}
