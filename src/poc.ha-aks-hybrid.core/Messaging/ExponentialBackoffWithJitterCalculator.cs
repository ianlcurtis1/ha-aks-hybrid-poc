//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Implements the IRetryDelayCalculator interface, which is used to calculate the delay between retries
    /// </summary>
    public class ExponentialBackoffWithJitterCalculator : IRetryDelayCalculator
    {
        private readonly Random random;
        private readonly object randomLock;

        public ExponentialBackoffWithJitterCalculator()
        {
            random = new Random();
            randomLock = new object();
        }
        public TimeSpan Calculate(int attemptNumber)
        {
            int jitter = 0;
            lock (randomLock) //because Random is not threadsafe
                jitter = random.Next(10, 200);

            return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber - 1)) + TimeSpan.FromMilliseconds(jitter);
        }
    }
}
