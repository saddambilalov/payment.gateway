namespace Payment.Gateway.Api.Clients.Contracts
{
    using Domain.ValueObjects;

    public class BankPaymentRequest
    {
        public double Amount { get; set; }

        public Currency Currency { get; set; }

        public CardDetails CardDetails { get; set; }
    }
}