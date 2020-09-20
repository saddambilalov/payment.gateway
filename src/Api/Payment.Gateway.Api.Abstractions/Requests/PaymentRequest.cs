namespace Payment.Gateway.Api.Abstractions.Requests
{
    using Domain.ValueObjects;

    public class PaymentRequest
    {
        public double Amount { get; set; }

        public Currency Currency { get; set; }

        public string Merchant { get; set; }

        public CardDetails CardDetails { get; set; }
    }
}
