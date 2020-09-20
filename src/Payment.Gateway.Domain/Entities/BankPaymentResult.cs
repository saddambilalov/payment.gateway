namespace Payment.Gateway.Domain.Entities
{
    using System;
    using ValueObjects;

    public class BankPaymentResult
    {
        public Guid TransactionId { get; set; }

        public PaymentStatus Status { get; set; }
    }
}