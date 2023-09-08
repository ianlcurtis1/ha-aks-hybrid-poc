//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Defines the RetryDelayCalculator interface, which is used to calculate the delay between retries
    /// </summary>
    public interface IRetryDelayCalculator
    {
        /// <summary>
        /// Calculates the delay between retries
        /// </summary>
        /// <param name="attemptNumber">The number of attempts so far</param>
        /// <returns>A timespan representing the delay before a retry</returns>
        public TimeSpan Calculate(int attemptNumber);
    }
}
