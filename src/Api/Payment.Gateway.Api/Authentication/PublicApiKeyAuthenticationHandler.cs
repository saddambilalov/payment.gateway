namespace Payment.Gateway.Api.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Constants;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Services;

    public class PublicApiKeyAuthenticationHandler : AuthenticationHandler<PublicApiKeyAuthenticationOptions>
    {

        private readonly IPublicApiKeyStorage _apiKeyStorage;
        private string _failureMessage;

        public PublicApiKeyAuthenticationHandler(
            IOptionsMonitor<PublicApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IPublicApiKeyStorage apiKeyStorage) : base(options, logger, encoder, clock)
        {
            _apiKeyStorage = apiKeyStorage ?? throw new ArgumentNullException(nameof(apiKeyStorage));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(PublicApiKeyConstants.HeaderName, out var apiKeyHeaderValues))
            {
                _failureMessage = PublicApiKeyConstants.PublicKeyIsMissing;
                return AuthenticateResult.NoResult();
            }

            var publicApiKeyHeaderValue = apiKeyHeaderValues.FirstOrDefault();

            if (!apiKeyHeaderValues.Any()
                || string.IsNullOrWhiteSpace(publicApiKeyHeaderValue))
            {
                _failureMessage = PublicApiKeyConstants.PublicKeyIsMissing;
                return AuthenticateResult.NoResult();
            }

            var keyDetails = await _apiKeyStorage.ValidateAsync(publicApiKeyHeaderValue);
            if (keyDetails == null)
            {
                _failureMessage = PublicApiKeyConstants.InvalidPublicKey;
                return AuthenticateResult.Fail(_failureMessage);
            }

            var claims = new List<Claim>
            {
                new Claim("Merchant", keyDetails.Merchant)
            };
            var claimsIdentity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity> { claimsIdentity });
            var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Options.Scheme);

            return AuthenticateResult.Success(authenticationTicket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = "application/problem+json";

            await Response.WriteAsync(JsonSerializer.Serialize(new
            {
                Title = "Unauthorized",
                Detail = _failureMessage,
                Status = 401
            }));
        }
    }
}
