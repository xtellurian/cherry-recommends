using System;

namespace SignalBox.Core
{
    public class CustomerMetricDailyStringAggregate
    {
        public CustomerMetricDailyStringAggregate()
        { }

        public DateTimeOffset CalendarDate { get; set; }
        public DateTimeOffset FirstOfWeek { get; set; }
        public DateTimeOffset LastOfWeek { get; set; }
        public long CustomerId { get; set; }
        public long MetricId { get; set; }
        public double StringValue { get; set; }
        public int ValueCount { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}