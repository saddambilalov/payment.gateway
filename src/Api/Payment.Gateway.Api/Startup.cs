using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Payment.Gateway.Api
{
    using System;
    using Abstractions.Requests;
    using Authentication;
    using Authentication.Services;
    using AutoMapper;
    using Clients;
    using Domain.ValueObjects;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Infrastructure.Profiles;
    using Infrastructure.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Profiles;
    using Refit;
    using Services;
    using Services.Interfaces;
    using Setup;
    using Setup.Options;
    using Validators;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation();

            services.AddSingleton<IPublicApiKeyStorage, InMemoryPublicApiKeyStorage>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = PublicApiKeyAuthenticationOptions.DefaultScheme;
                    options.DefaultChallengeScheme = PublicApiKeyAuthenticationOptions.DefaultScheme;
                })
                .AddPublicApiKeyValidation(options =>
                {
                });

            services.AddTransient<IValidator<CardDetails>, CardDetailsValidator>();
            services.AddTransient<IValidator<PaymentRequest>, PaymentRequestValidator>();

            var clientOptions = Configuration.GetSection("Clients:PollyOptions").Get<ClientOptions>();
            services.AddHttpClient("BankPayment", _ =>
                {
                    _.BaseAddress = Configuration.GetValue<Uri>("Clients:Endpoint");
                })
                .AddTypedClient(RestService.For<IBankClient>)
                .ApplyPolicy(clientOptions);

            services.AddDataProtection(Configuration);
            services.RegisterDataStore(Configuration);
            services.AddSwagger();

            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IBankPaymentIssuerService, BankPaymentIssuerService>();

            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile(provider.GetService<ICipherService>()));
                cfg.AddProfile(new PaymentResourceProfile());
            }).CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandlerHelper(env, loggerFactory);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway Api");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
