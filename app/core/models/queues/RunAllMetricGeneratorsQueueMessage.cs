namespace SignalBox.Core
{
    public class RunAllMetricGeneratorsQueueMessage : IQueueMessage
    {
        public RunAllMetricGeneratorsQueueMessage()
        { }
        public RunAllMetricGeneratorsQueueMessage(string tenantName)
        {
            this.TenantName = tenantName;
        }

        public string TenantName { get; set; }
    }
}