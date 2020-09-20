namespace Payment.Gateway.Infrastructure.DataPersistence.Configuration
{
    public class PaymentGatewayDbSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }
    }
}