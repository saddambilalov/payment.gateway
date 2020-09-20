namespace Payment.Gateway.Api.Setup
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Polly;
    using Polly.Extensions.Http;

    public static class PollyExtension
    {
        public static void ApplyPolicy(this IHttpClientBuilder clientBuilder, ClientOptions clientConfiguration)
        {
            if (clientConfiguration.EnableCircuitBreaker)
            {
                clientBuilder
                    .AddTransientHttpErrorPolicy(builder =>
                        builder.JitterRetryAsync(clientConfiguration.Retries))
                    .AddTransientHttpErrorPolicy(builder => builder.AdvancedCircuitBreakerAsync(
                        clientConfiguration.FailureThreshold,
                        TimeSpan.FromSeconds(clientConfiguration.SamplingDuration),
                        clientConfiguration.MinimumThreshold,
                        TimeSpan.FromSeconds(clientConfiguration.DurationOfBreak)));
            }
        }

        public static IAsyncPolicy<HttpResponseMessage> JitterRetryAsync(
            this PolicyBuilder<HttpResponseMessage> builder,
            int retries
        )
        {
            var jitterer = new Random();
            var retryWithJitterPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );

            return retryWithJitterPolicy;
        }
    }
}