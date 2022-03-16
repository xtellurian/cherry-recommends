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
        }

        public long SegmentId { get; set; }
        public long RuleId { get; set; }
        public int CustomersEnrolled { get; set; }
    }
}