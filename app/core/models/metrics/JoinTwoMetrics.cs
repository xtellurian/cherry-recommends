namespace SignalBox.Core.Metrics
{
    public class JoinTwoMetrics
    {
        public long Metric1Id { get; set; }
        public Metric Metric1 { get; set; }
        public long Metric2Id { get; set; }
        public Metric Metric2 { get; set; }
        public JoinType JoinType { get; set; }
    }
}