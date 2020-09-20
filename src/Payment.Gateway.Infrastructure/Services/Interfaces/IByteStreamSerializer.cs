namespace Payment.Gateway.Infrastructure.Services.Interfaces
{
    public interface IByteStreamSerializer
    {
        TOutput Deserialize<TOutput>(byte[] arrayToDeserialize) where TOutput : new();
        byte[] Serialize<TInput>(TInput objectToSerialize) where TInput : new();
    }
}