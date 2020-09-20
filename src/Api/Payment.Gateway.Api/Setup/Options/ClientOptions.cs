namespace Payment.Gateway.Api.Setup.Options
{
    public class ClientOptions
    {
        public int Retries { get; set; }

        public bool EnableCircuitBreaker { get; set; }

        public double FailureThreshold { get; set; }

        public int SamplingDuration { get; set; }

        public int MinimumThreshold { get; set; }

        public int DurationOfBreak { get; set; }
    }
}