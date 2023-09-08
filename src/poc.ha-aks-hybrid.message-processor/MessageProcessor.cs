using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PoC.HaAKSHybrid.Tests")]

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid.MessageProcessor
{
    internal class MessageProcessor : IMessageProcessor
    {
        private readonly IOptions<Config> _config;
        private AsyncRetryPolicy? _retryPolicy;
        

        public MessageProcessor(IOptions<Config> config)
        {
            _config = config;
        }

        /// <summary>
        /// Process a queue of messages using the circuit-breaker pattern
        /// </summary>
        /// <param name="queue">Queue implementation</param>
        /// <param name="proxy">Proxy implementation</param>
        /// <returns></returns>
        public async Task ProcessMessageQueueAsync(IQueue queue, IProxy proxy)
        {
            Console.WriteLine("processing message queue...");

            _retryPolicy = Policy.Handle<TransientException>()
                .WaitAndRetryAsync(
                retryCount: _config.Value.CircuitBreakerRetries,
                sleepDurationProvider: (attemptCount) => new ExponentialBackoffWithJitterCalculator().Calculate(attemptCount),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                    Console.WriteLine($"transient error: {exception.Message}\n retrying in {sleepDuration}. {attemptNumber} / {_config.Value.CircuitBreakerRetries}");
                });

            
            while (true)
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var fileProcessed = await ProcessNextMessageAsync(queue, proxy);
                    if (!fileProcessed)
                    {
                        Console.WriteLine("no message found, sleeping 2000ms");
                        Thread.Sleep(2000);
                    }
                });
            }
        }

        /// <summary>
        /// Process the next message in the queue
        /// </summary>
        /// <param name="queue">Queue implementation</param>
        /// <param name="proxy">Proxy implementation</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal async Task<bool> ProcessNextMessageAsync(IQueue queue, IProxy proxy)
        {
            var file = await queue.PeekAsync();

            var fileFound = !string.IsNullOrEmpty(file.Item1);
            if (fileFound)
            {
                //check validity

                if (await proxy.SendMessageAsync(file.Item1, file.Item2))
                {
                    if (!await queue.DequeueAsync(file.Item1))
                    {
                        //implement poison queue otherwise we'll get stuck on this message!
                        Console.WriteLine($"unable to dequeue message {file.Item1}");
                    }
                    else
                    {
                        Console.WriteLine($"file {file.Item1} dequeued.");
                    }
                }
                else 
                {
                    //implement poison queue otherwise we'll get stuck on this message!
                }
            }

            return fileFound;
        }
    }
}
