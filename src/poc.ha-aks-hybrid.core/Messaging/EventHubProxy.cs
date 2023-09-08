using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using System.Text;

//https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send?tabs=connection-string%2Croles-azure-portal

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Implements an IProxy, which sends messages to a server event hub.
    /// </summary>
    public class EventHubProxy : IProxy
    {
        private readonly IOptions<Config> _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration parameters</param>
        public EventHubProxy(IOptions<Config> config) 
        { 
            _config = config;
        }

        public async Task<bool> SendMessageAsync(string id, string message)
        {
            //throw new TransientException("test exception");
            var ok = false;

            if (string.IsNullOrEmpty(_config.Value.EventHubConnectionString))
                throw new IncompleteConfigurationException($"EventHubConnectionString not configured in secrets.");

            EventHubProducerClient producerClient = new EventHubProducerClient(_config.Value.EventHubConnectionString, _config.Value.HubName);
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message))))
                Console.WriteLine($"message {id} is too large to send.");

            try
            {
                await producerClient.SendAsync(eventBatch);
                ok = true;
            }
            catch (Exception ex)
            {
                //catch with retry logic
                throw new TransientException(ex.Message);   
            }
            finally
            {
                await producerClient.DisposeAsync();
            }

            return ok;
        }
    }
}
