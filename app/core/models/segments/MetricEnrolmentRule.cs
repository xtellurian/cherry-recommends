using SignalBox.Core.Predicates;

namespace SignalBox.Core.Segments
{
#nullable enable
    public class MetricEnrolmentRule : EnrolmentRule
    {
        public long? MetricId { get; set; }
        public Metric? Metric { get; set; }
        public NumericPredicate? NumericPredicate { get; set; }

        public static MetricEnrolmentRule MoreThan10EventsEnrolmentRule => new MetricEnrolmentRule
        {
            Id = 100,
            MetricId = Metric.TotalEvents.Id,
            SegmentId = Segment.MoreThan10Events.Id,
            NumericPredicate = new NumericPredicate(NumericPredicateOperators.GreaterThan, 10)
        };
    }
}