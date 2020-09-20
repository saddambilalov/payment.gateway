namespace Payment.Gateway.Api.Abstractions.Responses
{
    using System;
    using Resources;

    public class PaymentResponse
    {
        public CardDetailsResource CardDetails { get; set; }

        public Guid TransactionId { get; set; }

        public string PaymentStatus { get; set; }
    }
}
