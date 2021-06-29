namespace SignalBox.Infrastructure
{
    public class AzureQueueConfig : AzureStorageConfig
    {
        public bool EnableReadQueue { get; set; }
        public bool EnableWriteQueue { get; set; }

    }
}