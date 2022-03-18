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
        public string StringValue { get; set; }
        /// <summary>
        /// The sum of the occurrance of a particular string value in a week
        /// </summary>
        public int WeeklyValueCount { get; set; }
        /// <summary>
        /// The sum of distinct customers with a string value in a week
        /// </summary>
        public int WeeklyDistinctCustomerCount { get; set; }
        public int WeeklyDistinctBusinessCount { get; set; }
    }
}