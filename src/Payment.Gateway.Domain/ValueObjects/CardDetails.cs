namespace Payment.Gateway.Domain.ValueObjects
{
    using System;

    [Serializable]
    public class CardDetails
    {
        public string CardNumber { get; set; }

        public int CardExpiryYear { get; set; }

        public int CardExpiryMonth { get; set; }

        public int Cvv { get; set; }
    }
}