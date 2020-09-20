namespace Payment.Gateway.Api.Setup
{
    using System;
    using Domain.Repositories;
    using Infrastructure.DataPersistence.Configuration;
    using Infrastructure.Repositories;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class DataStoreExtension
    {
        public static IServiceCollection RegisterDataStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaymentGatewayDbSettings>(
                configuration.GetSection(nameof(PaymentGatewayDbSettings)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<PaymentGatewayDbSettings>>()?.Value ??
                throw new ArgumentNullException(nameof(PaymentGatewayDbSettings)));

            services.AddSingleton<IPaymentRepository, PaymentRepository>();

            return services;
        }
    }
}