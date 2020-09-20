namespace Payment.Gateway.Infrastructure.Services
{
    using Interfaces;
    using Microsoft.AspNetCore.DataProtection;
    using Settings;

    public class CipherService : ICipherService
    {
        private readonly IByteStreamSerializer _serializer;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly CipherSettings _cipherSettings;

        public CipherService(
            IByteStreamSerializer serializer,
            IDataProtectionProvider dataProtectionProvider,
            CipherSettings cipherSettings)
        {
            _serializer = serializer;
            _dataProtectionProvider = dataProtectionProvider;
            _cipherSettings = cipherSettings;
        }

        public byte[] Encrypt<TInput>(TInput input) where TInput : new()
        {
            var protector = this._dataProtectionProvider.CreateProtector(_cipherSettings.Key);
            var plainText = _serializer.Serialize(input);
            return protector.Protect(plainText);
        }

        public TOutput Decrypt<TOutput>(byte[] cipherText) where TOutput : new()
        {
            var protector = this._dataProtectionProvider.CreateProtector(_cipherSettings.Key);
            var bytes = protector.Unprotect(cipherText);

            return _serializer.Deserialize<TOutput>(bytes);
        }
    }
}