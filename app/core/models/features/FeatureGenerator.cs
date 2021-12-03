using System.Collections.Generic;
using SignalBox.Core.Features;

namespace SignalBox.Core
{
    public enum FeatureGeneratorTypes
    {
        MonthsSinceEarliestEvent,
        FilterSelectAggregate,
    }

    public class FeatureGenerator : Entity
    {
        protected FeatureGenerator()
        { }
        protected FeatureGenerator(Feature feature, FeatureGeneratorTypes generatorType)
        {
            this.Feature = feature;
            this.FeatureId = feature.Id;
            this.GeneratorType = generatorType;
        }
        public FeatureGenerator(Feature feature, FeatureGeneratorTypes generatorType, IEnumerable<FilterSelectAggregateStep> steps)
        : this(feature, generatorType)
        {
            this.FilterSelectAggregateSteps = new List<FilterSelectAggregateStep>(steps);
        }

        public System.DateTimeOffset? LastEnqueued { get; set; }
        public System.DateTimeOffset? LastCompleted { get; set; }
        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
        public FeatureGeneratorTypes GeneratorType { get; set; }
        public List<FilterSelectAggregateStep> FilterSelectAggregateSteps { get; set; }
    }
}