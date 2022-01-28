using System.Collections.Generic;
using SignalBox.Core.Metrics;

namespace SignalBox.Core
{
    public enum MetricGeneratorTypes
    {
        MonthsSinceEarliestEvent,
        FilterSelectAggregate,
    }

    public class MetricGenerator : Entity
    {
        protected MetricGenerator()
        { }
        protected MetricGenerator(Metric metric, MetricGeneratorTypes generatorType)
        {
            this.Metric = metric;
            this.MetricId = metric.Id;
            this.GeneratorType = generatorType;
        }
        public MetricGenerator(Metric metric, MetricGeneratorTypes generatorType, IEnumerable<FilterSelectAggregateStep> steps)
        : this(metric, generatorType)
        {
            this.FilterSelectAggregateSteps = new List<FilterSelectAggregateStep>(steps);
        }

        public System.DateTimeOffset? LastEnqueued { get; set; }
        public System.DateTimeOffset? LastCompleted { get; set; }
        public long MetricId { get; set; }
        public Metric Metric { get; set; }
        public Metric Feature => Metric;
        public MetricGeneratorTypes GeneratorType { get; set; }
        public List<FilterSelectAggregateStep> FilterSelectAggregateSteps { get; set; }
    }
}