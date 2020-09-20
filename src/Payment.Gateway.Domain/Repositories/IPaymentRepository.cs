namespace Payment.Gateway.Domain.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment, CancellationToken token);

        Task<Payment> FindByTransactionIdAsync(Guid transactionId, CancellationToken token);
    }
}
