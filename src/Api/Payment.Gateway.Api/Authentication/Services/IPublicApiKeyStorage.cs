namespace Payment.Gateway.Api.Authentication.Services
{
    using System.Threading.Tasks;

    public interface IPublicApiKeyStorage
    {
        Task<PublicApiKey> ValidateAsync(string apiKey);
    }
}
