namespace SignalBox.Infrastructure
{
    public abstract class AzureStorageConfig
    {
        public string ConnectionString { get; set; } // only set when blob

    }
}