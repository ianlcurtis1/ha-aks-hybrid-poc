//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Defines the MessageProcessor interface, which processes a queue of messages to send to server />
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Processes the message queue asynchronously.
        /// </summary>
        /// <param name="queue">The queue implementation</param>
        /// <param name="proxy">The proxy implementation</param>
        /// <returns></returns>
        Task ProcessMessageQueueAsync(IQueue queue, IProxy proxy);
    }
}
