//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// An exception that indicates that a transient error has occurred and the operation should be retried
    /// </summary>
    public sealed class TransientException : Exception
    {
        public TransientException(string msg) : base(msg) { }
    }

    /// <summary>
    /// An exception that indicates that the configuration is incomplete and the process cannot continue
    /// </summary>
    public sealed class IncompleteConfigurationException : Exception
    {
        public IncompleteConfigurationException(string msg) : base(msg) { }
    }
}
