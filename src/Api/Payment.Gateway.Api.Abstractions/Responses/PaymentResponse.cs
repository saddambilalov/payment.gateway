namespace Payment.Gateway.Api.Abstractions.Responses
{
    using System;
    using Domain.ValueObjects;
    using Resources;

    public class PaymentResponse
    {
        public CardDetailsResource CardDetails { get; set; }

        public Guid TransactionId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
