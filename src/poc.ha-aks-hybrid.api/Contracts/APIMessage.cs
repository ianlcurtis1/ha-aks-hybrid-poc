namespace PoC.HaAKSHybrid.API
{
    /// <summary>
    /// Dto for messages to be sent to REST API
    /// </summary>
    public class APIMessageIn
{
        public string SenderId { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public APIMessageIn() { }

        public APIMessageIn(string senderId, string value)
        {
            Value = value;
            SenderId = senderId;
        }
    }

    /// <summary>
    /// Dto for messages to be sent from REST API
    /// </summary>
    public class APIMessageOut
    {
        public string SenderId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Success { get; set; } = false;

        public APIMessageOut() { }

        public APIMessageOut(string senderId, string correlationId, string value, bool success)
        {
            SenderId = senderId;
            CorrelationId = correlationId;
            Value = value;
            Success = success;
        }
    }
}
