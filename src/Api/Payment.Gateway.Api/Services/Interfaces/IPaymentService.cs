namespace Payment.Gateway.Api.Services.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Requests;
    using Abstractions.Responses;

    public interface IPaymentService
    {
        Task<PaymentIssuedResponse> ProceedPaymentAsync(PaymentRequest paymentRequest, CancellationToken cancellationToken);
        Task<PaymentResponse> GetPaymentDetailsWithTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken);
    }
}