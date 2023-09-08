//https://redis.io/docs/data-types/lists/

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    public class RedisQueue : IQueue
    {
        public Task<bool> DequeueAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EnqueueAsync(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<string, string>> PeekAsync()
        {
            throw new NotImplementedException();
        }
    }
}