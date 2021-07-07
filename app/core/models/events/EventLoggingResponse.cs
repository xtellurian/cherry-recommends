namespace SignalBox.Core
{
    public class EventLoggingResponse
    {
        public int EventsProcessed { get; set; }
        public int ActionsProcessed { get; set; }
        public int EventsEnqueued { get; set; }
    }
}