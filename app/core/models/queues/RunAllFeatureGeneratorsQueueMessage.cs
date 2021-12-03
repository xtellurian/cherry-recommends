namespace SignalBox.Core
{
    public class RunAllFeatureGeneratorsQueueMessage : IQueueMessage
    {
        public RunAllFeatureGeneratorsQueueMessage()
        { }
        public RunAllFeatureGeneratorsQueueMessage(string tenantName)
        {
            this.TenantName = tenantName;
        }

        public string TenantName { get; set; }
    }
}