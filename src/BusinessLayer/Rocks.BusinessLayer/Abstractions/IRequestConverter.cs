namespace Rocks.BusinessLayer.Abstractions
{
    public interface IRequestConverter<T>
    {
        byte[] Convert(T request);
    }
}
