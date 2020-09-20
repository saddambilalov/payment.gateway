namespace Payment.Gateway.Api.Authentication
{
    using Microsoft.AspNetCore.Authentication;

    public class PublicApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "Public API Key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
