namespace Payment.Gateway.Api.Abstractions.Responses
{
    using System;
    using Domain.ValueObjects;

    public class PaymentIssuedResponse
    {
        public Guid TransactionId { get; set; }

        public string Status { get; set; }
    }
}