namespace SignalBox.Core
{
    public class RunAllHubspotDataPushQueueMessage : TenantJobMessageBase
    {
        public RunAllHubspotDataPushQueueMessage(string tenantName) : base(tenantName)
        { }
    }
}