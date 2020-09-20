namespace Payment.Gateway.Api.Services.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Requests;
    using Domain.Entities;

    public interface IBankPaymentIssuerService
    {
        Task<BankPaymentResult> IssuePaymentAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken);
    }
}