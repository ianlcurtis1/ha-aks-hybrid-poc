using Microsoft.Extensions.Options;

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    public class DiskQueue : IQueue
    {
        private readonly IOptions<Config> _config;

        public DiskQueue(IOptions<Config> config)
        {
            _config = config;
        }

        public async Task<bool> EnqueueAsync(Message message)
        {
            if (string.IsNullOrEmpty(_config.Value.QueueDirectory))
                throw new IncompleteConfigurationException("QueueDirectory not configured in settings.");

            if (!Directory.Exists(_config.Value.QueueDirectory))
            {
                Console.WriteLine($"creating queue directory {_config.Value.QueueDirectory}");
                Directory.CreateDirectory(_config.Value.QueueDirectory);
            }

            var f = $@"{_config.Value.QueueDirectory}/{message.CorrelationId}.json";

            Console.WriteLine($@"queueing to {f}");

            await File.WriteAllTextAsync(f, message.SenderId + "\n" + message.Value);
            return true;
        }

        public async Task<Tuple<string, string>> PeekAsync()
        {
            //doesn't handle contention!
            var file = string.Empty;
            var contents = string.Empty;
            
            if (string.IsNullOrEmpty(_config.Value.QueueDirectory))
                throw new IncompleteConfigurationException($"QueueDirectory not configured in settings.");

            if (!Directory.Exists(_config.Value.QueueDirectory))
            {
                Console.WriteLine($"directory {_config.Value.QueueDirectory} not found when peeking.");
            }
            else
            {
                if (Directory.GetFiles(_config.Value.QueueDirectory).Length > 0)
                {
                    file = Directory.GetFiles(_config.Value.QueueDirectory).First();

                    Console.WriteLine($"reading file {file}");

                    contents = await File.ReadAllTextAsync(file);
                }
            }

            return new Tuple<string, string>(file, contents);
        }

        public async Task<bool> DequeueAsync(string key)
        {
            Console.WriteLine($"deleting file {key}");

            try
            {
                //doesn't handle contention!
                if (File.Exists(key)) File.Delete(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error deleting file {key}: {ex.Message}");
                return false;
            }

            return true;
        }
    }
}
