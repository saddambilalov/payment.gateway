namespace Payment.Gateway.Api.Setup
{
    using System;
    using Authentication;
    using Microsoft.AspNetCore.Authentication;

    public static class PublicApiKeyExtension
    {
        public static AuthenticationBuilder AddPublicApiKeyValidation(this AuthenticationBuilder authenticationBuilder, Action<PublicApiKeyAuthenticationOptions> options)
        {
            return authenticationBuilder.AddScheme<PublicApiKeyAuthenticationOptions, PublicApiKeyAuthenticationHandler>(PublicApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }
}
