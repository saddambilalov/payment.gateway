namespace Payment.Gateway.Api.Authentication.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryPublicApiKeyStorage : IPublicApiKeyStorage
    {
        private readonly IList<PublicApiKey> _apiKeys;

        public InMemoryPublicApiKeyStorage()
        {
            _apiKeys = new List<PublicApiKey>
            {
                new PublicApiKey
                {
                    PublicKey = "285c0a57-d2f7-47fc-9e54-fc61d0d15fa3",
                    Merchant = "Apple"
                },
                new PublicApiKey
                {
                    PublicKey = "3c93f7cd-77be-412f-8199-288037ba8321",
                    Merchant = "Amazon"
                }
            };
        }

        public Task<PublicApiKey> ValidateAsync(string apiKey)
        {
            var key = _apiKeys.FirstOrDefault(_ => _.PublicKey == apiKey);
            return Task.FromResult(key);
        }
    }
}
