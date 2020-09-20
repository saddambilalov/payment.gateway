namespace Payment.Gateway.Api.Setup
{
    using Authentication;
    using Authentication.Constants;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway Api",
                    Description = "Documentation for Payment Gateway Api",
                });

                _.AddSecurityDefinition(PublicApiKeyConstants.HeaderName, new OpenApiSecurityScheme
                {
                    Description = $"Public Api Key is required. Please provide: {PublicApiKeyConstants.HeaderName}",
                    In = ParameterLocation.Header,
                    Name = PublicApiKeyConstants.HeaderName,
                    Type = SecuritySchemeType.ApiKey
                });

                _.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = PublicApiKeyConstants.HeaderName,
                                Type = SecuritySchemeType.ApiKey,
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                    {Type = ReferenceType.SecurityScheme, Id = PublicApiKeyConstants.HeaderName}
                            },
                            new string[] { }
                        }
                });
            });

            return services;
        }
    }
}