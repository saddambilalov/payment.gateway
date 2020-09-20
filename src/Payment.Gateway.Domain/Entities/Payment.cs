namespace Payment.Gateway.Domain.Entities
{
    using System;
    using ValueObjects;

    public class Payment
    {
        public Guid Id { get; set; }

        public string Merchant { get; set; }

        public double Amount { get; set; }

        public Currency Currency { get; set; }

        public CardDetails CardDetails { get; set; }

        public BankPaymentResult BankPaymentResult { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}