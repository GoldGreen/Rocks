namespace Rabbit.Rpc.Abstractions
{
    public interface IResponceConverter<T>
    {
        T Convert(byte[] data);
    }
}
