namespace SignalBox.Infrastructure.Models
{
    public abstract class EventhubConfig
    {
        public string ConnectionString { get; set; }
        public string EventhubName { get; set; }
    }
}