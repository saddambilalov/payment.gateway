namespace Payment.Gateway.Api.Authentication.Constants
{
    public static class PublicApiKeyConstants
    {
        public const string HeaderName = "Authorization";

        public const string PublicKeyIsMissing = "Public API Key is missing";

        public const string InvalidPublicKey = "Invalid or expired Public API Key provided.";
    }
}
