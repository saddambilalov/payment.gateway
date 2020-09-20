namespace Payment.Gateway.Infrastructure.Services.Interfaces
{
    public interface ICipherService
    {
        byte[] Encrypt<TInput>(TInput input) where TInput : new();
        TOutput Decrypt<TOutput>(byte[] cipherText) where TOutput : new();
    }
}
