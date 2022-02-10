using System;

namespace SignalBox.Core
{
    public class CustomerMetricWeeklyStringAggregate
    {
        public CustomerMetricWeeklyStringAggregate()
        { }

        public DateTimeOffset FirstOfWeek { get; set; }
        public DateTimeOffset LastOfWeek { get; set; }
        public long MetricId { get; set; }
        public string StringValue {get;set;}
        public int WeeklyValueCount { get; set; }
        public int WeeklyDistinctCustomerCount { get; set; }
    }
}