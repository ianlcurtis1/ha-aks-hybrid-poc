namespace PoC.HaAKSHybrid
{
    /// <summary>
    /// Dto for messages to be sent to server
    /// </summary>
    public class Message
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;

        public Message(string senderId, string value)
        {
            CorrelationId = Guid.NewGuid().ToString();
            Value = value;
            SenderId = senderId;
        }
    }

    public static class MessageExtensions
    {
        public static bool IsValid(this Message value)
        {
            if (value == null) return false;
            if (string.IsNullOrEmpty(value.Value)) return false;
            if (string.IsNullOrEmpty(value.SenderId)) return false;

            return true;
        }
    }
}
