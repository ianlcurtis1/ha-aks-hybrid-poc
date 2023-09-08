//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Defines the Queue interface, which is used to store messages to send to server
    /// </summary>
    public interface IQueue
    {
        /// <summary>
        /// Enqueues the message asynchronously.
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>Success or failure</returns>
        Task<bool> EnqueueAsync(Message message);

        /// <summary>
        /// Peeks at the message queue, and returns a message if one is found.
        /// </summary>
        /// <returns>A tuple containing the message Id and text</returns>
        Task<Tuple<string, string>> PeekAsync();

        /// <summary>
        /// Dequeues the message asynchronously.
        /// </summary>
        /// <param name="key">The message key</param>
        /// <returns>Success or failure</returns>
        Task<bool> DequeueAsync(string key);
    }
}
