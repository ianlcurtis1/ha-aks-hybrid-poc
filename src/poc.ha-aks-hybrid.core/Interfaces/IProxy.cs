//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Interface for messaging proxy, which sends messages to server
    /// </summary>
    public interface IProxy
    {
        /// <summary>
        /// Sends the message asynchronously.
        /// </summary>
        /// <param name="id">The message Id</param>
        /// <param name="message">The message</param>
        /// <returns>Success or failure</returns>
        Task<bool> SendMessageAsync(string id, string message);
    }
}
