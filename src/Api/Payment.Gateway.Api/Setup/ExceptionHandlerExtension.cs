namespace Payment.Gateway.Api.Setup
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public static class ExceptionHandlerExtension
    {
        public static void UseExceptionHandlerHelper(this IApplicationBuilder appBuilder, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            appBuilder.UseExceptionHandler(options =>
            {
                options.Run(
                    async httpContext =>
                    {
                        const string defaultErrorMessage = "Something went wrong! Please consult application logs.";
                        var logger = loggerFactory.CreateLogger(nameof(ExceptionHandlerExtension));

                        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                        var exception = exceptionHandlerFeature?.Error;

                        if (exception != null)
                        {
                            logger.LogError(exception.Message, exception);

                            httpContext.Response.ContentType = "application/problem+json";

                            var title = env.IsDevelopment()
                                ? $"An exception occurred: {exception.Message}"
                                : defaultErrorMessage;
                            var details = env.IsDevelopment() ? exception.ToString() : defaultErrorMessage;

                            var stream = httpContext.Response.Body;
                            await JsonSerializer.SerializeAsync(stream,
                                new
                                {
                                    Status = 500,
                                    Title = title,
                                    Detail = details
                                });
                        }
                    });
            });
        }
    }
}