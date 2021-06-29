namespace SignalBox.Infrastructure
{
    public class FileHosting : AzureStorageConfig
    {
        public string Source { get; set; } // can be local or blob
        public string ContainerName { get; set; } // only set when blob
        public string SubPath { get; set; }
    }
}