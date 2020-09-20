namespace Payment.Gateway.Api.Clients
{
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;
    using Refit;

    public interface IBankClient
    {
        [Post("/process-payment")]
        Task<BankPaymentResponse> ProcessPaymentAsync(BankPaymentRequest bankPaymentRequest, CancellationToken token);
    }
}