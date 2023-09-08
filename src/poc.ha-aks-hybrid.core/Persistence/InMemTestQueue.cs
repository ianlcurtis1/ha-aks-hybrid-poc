using Microsoft.Extensions.Options;

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    public class InMemTestQueue : IQueue
    {
        private readonly IOptions<Config> _config;

        public InMemTestQueue(IOptions<Config> config)
        {
            _config = config;
        }
        public async Task<bool> EnqueueAsync(Message message)
        {
            return true;
        }

        public async Task<Tuple<string, string>> PeekAsync()
        {
            return new Tuple<string, string>($"testfile{Guid.NewGuid().ToString()}", "empty file");
        }

        public async Task<bool> DequeueAsync(string key)
        {
            return true;
        }
    }
}
