using System;

namespace SignalBox.Core
{
    public class CustomerMetricWeeklyNumericAggregate
    {
        public CustomerMetricWeeklyNumericAggregate()
        { }

        public DateTimeOffset FirstOfWeek { get; set; }
        public DateTimeOffset LastOfWeek { get; set; }
        public long MetricId { get; set; }
        public double WeeklyMeanNumericValue { get; set; }
        public int WeeklyDistinctCustomerCount { get; set; }
    }
}