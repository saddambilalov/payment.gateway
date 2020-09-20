namespace Payment.Gateway.Api.Abstractions.Resources
{
    public class CardDetailsResource
    {
        public string CardNumber { get; set; }

        public int CardExpiryYear { get; set; }

        public int CardExpiryMonth { get; set; }
    }
}
