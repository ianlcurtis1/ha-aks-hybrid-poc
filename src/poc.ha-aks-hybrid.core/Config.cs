//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    public class Config
    {
        //public string? KeyVaultName { get; set; }
        public string? EventHubConnectionString { get; set; }
        public string? QueueDirectory { get; set; }
        public string? HubName { get; set; }
        public int CircuitBreakerRetries { get; set; } = 5;
    }
}
