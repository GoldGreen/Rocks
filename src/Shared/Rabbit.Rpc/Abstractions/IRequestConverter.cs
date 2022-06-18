namespace Rabbit.Rpc.Abstractions
{
    public interface IRequestConverter<T>
    {
        byte[] Convert(T request);
    }
}
