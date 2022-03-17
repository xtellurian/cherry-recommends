namespace SignalBox.Core
{
    public class RunAllSegmentEnrolmentRulesQueueMessage : TenantJobMessageBase
    {

        public RunAllSegmentEnrolmentRulesQueueMessage(string tenantName) : base(tenantName)
        { }
    }
}