using SignalBox.Core.Segments;

namespace SignalBox.Core
{
    public struct EnrolmentReport
    {
        public EnrolmentReport(EnrolmentRule rule)
        {
            SegmentId = rule.SegmentId;
            RuleId = rule.Id;
            CustomersEnrolled = 0;
            Rule = rule;
        }

        public long SegmentId { get; }
        public long RuleId { get; }
        public EnrolmentRule Rule { get; }
        public int CustomersEnrolled { get; private set; }

        public void IncrementCustomers()
        {
            CustomersEnrolled += 1;
        }
    }
}