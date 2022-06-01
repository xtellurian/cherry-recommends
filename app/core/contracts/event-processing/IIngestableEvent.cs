namespace SignalBox.Core
{
    public interface IIngestableEvent
    {
        public string EventId { get; }
        public string TenantName { get; set; } // required for sending messages to dotnetFunctions
    }
}