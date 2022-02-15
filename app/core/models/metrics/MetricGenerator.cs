using System.Collections.Generic;
using SignalBox.Core.Metrics;

namespace SignalBox.Core
{
    public enum MetricGeneratorTypes
    {
        MonthsSinceEarliestEvent,
        FilterSelectAggregate,
        AggregateCustomerMetric,
    }

    public class MetricGenerator : Entity
    {
        private const long totalEventsGeneratorId = 100;
        private static List<FilterSelectAggregateStep> TotalEventsGeneratorSteps => new List<FilterSelectAggregateStep>
        {
            new FilterSelectAggregateStep(1, new SelectStep(null)),
            new FilterSelectAggregateStep(2, new AggregateStep(){ AggregationType = AggregationTypes.Sum } )
        };
        public static MetricGenerator TotalEventsGenerator => new MetricGenerator(Metric.TotalEvents.Id, MetricGeneratorTypes.FilterSelectAggregate, TotalEventsGeneratorSteps)
        {
            Id = totalEventsGeneratorId,
        };

        public static MetricGenerator ForAggregateCustomerMetric(Metric metric, AggregateCustomerMetric definition)
        {
            return new MetricGenerator
            {
                MetricId = metric.Id,
                Metric = metric,
                GeneratorType = MetricGeneratorTypes.AggregateCustomerMetric,
                AggregateCustomerMetric = definition
            };
        }

        protected MetricGenerator()
        { }
        protected MetricGenerator(Metric metric, MetricGeneratorTypes generatorType)
        {
            this.Metric = metric;
            this.MetricId = metric.Id;
            this.GeneratorType = generatorType;
        }
        protected MetricGenerator(long metricId, MetricGeneratorTypes generatorType, IEnumerable<FilterSelectAggregateStep> steps) // added for seed data
        {
            this.MetricId = metricId;
            this.GeneratorType = generatorType;
            this.FilterSelectAggregateSteps = new List<FilterSelectAggregateStep>(steps);
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

#nullable enable
        public AggregateCustomerMetric? AggregateCustomerMetric { get; set; }
        public JoinTwoMetrics? JoinTwoMetrics { get; set; }
    }
}
