using System;

namespace SignalBox.Core
{
    public class CustomerMetricDailyNumericAggregate
    {
        public CustomerMetricDailyNumericAggregate()
        { }

        public DateTimeOffset CalendarDate { get; set; }
        public DateTimeOffset FirstOfWeek { get; set; }
        public DateTimeOffset LastOfWeek { get; set; }
        public long CustomerId { get; set; }
        public long MetricId { get; set; }
        public double NumericValue { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}