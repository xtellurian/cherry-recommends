namespace SignalBox.Core
{
    public class RunSegmentEnrolmentRuleQueueMessage : TenantJobMessageBase
    {
        public RunSegmentEnrolmentRuleQueueMessage()
        { }
        public RunSegmentEnrolmentRuleQueueMessage(string tenantName, long ruleId, long? environmentId) : base(tenantName)
        {
            RuleId = ruleId;
            EnvironmentId = environmentId;
        }

        public long RuleId { get; set; }
        public long? EnvironmentId { get; set; }

    }
}
