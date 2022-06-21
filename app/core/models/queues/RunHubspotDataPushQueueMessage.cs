namespace SignalBox.Core
{
    public class RunHubspotDataPushQueueMessage : TenantJobMessageBase
    {
        public RunHubspotDataPushQueueMessage()
        { }
        public RunHubspotDataPushQueueMessage(string tenantName, long integratedSystemId, long? environmentId) : base(tenantName)
        {
            IntegratedSystemId = integratedSystemId;
            EnvironmentId = environmentId;
        }

        public long IntegratedSystemId { get; set; }
        public long? EnvironmentId { get; set; }
    }
}
