using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid.MessageProcessor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("starting PoC.HaAKSHybrid.MessageProcessor");

            try
            {
                var serviceProvider = Configure();

                //handle continuing failures that will eventually bubble up after the number of retries has been exceeded!
                var mp = serviceProvider.GetService<IMessageProcessor>();
                await mp.ProcessMessageQueueAsync(serviceProvider.GetService<IQueue>(), serviceProvider.GetService<IProxy>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"process cannot continue, unhandled exception: {ex.Message}");
            }
        }

        private static ServiceProvider Configure()
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appSettings.json")
              .AddEnvironmentVariables()
              .AddUserSecrets<Config>(true)
              .Build();

            return new ServiceCollection()
                .AddTransient<IMessageProcessor, MessageProcessor>()
                .AddTransient<IQueue, DiskQueue>()
                .AddTransient<IProxy, EventHubProxy>()
                .Configure<Config>(configuration)
                .BuildServiceProvider();
        }
    }
}

