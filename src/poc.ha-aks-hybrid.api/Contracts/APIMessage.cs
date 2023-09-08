namespace PoC.HaAKSHybrid.API
{
    /// <summary>
    /// Dto for messages to be sent to REST API
    /// </summary>
    public class APIMessage
{
        public string SenderId { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public APIMessage() { }

        public APIMessage(string senderId, string value)
        {
            Value = value;
            SenderId = senderId;
        }
    }
}
