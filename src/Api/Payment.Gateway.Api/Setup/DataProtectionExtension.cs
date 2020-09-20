namespace Payment.Gateway.Api.Setup
{
    using System;
    using Infrastructure.Services;
    using Infrastructure.Services.Interfaces;
    using Infrastructure.Services.Settings;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class DataProtectionExtension
    {
        public static IServiceCollection AddDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            services.Configure<CipherSettings>(
                configuration.GetSection(nameof(CipherSettings)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<CipherSettings>>()?.Value ??
                throw new ArgumentNullException(nameof(CipherSettings)));

            services.AddSingleton<ICipherService, CipherService>();
            services.AddSingleton<IByteStreamSerializer, ByteStreamSerializer>();

            return services;
        }
    }
}