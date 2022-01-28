using System.Text.Json.Serialization;

namespace SignalBox.Core.Metrics
{
    public enum FilterSelectAggregateStepType
    {
        Filter,
        Select,
        Aggregate
    }

    public class FilterSelectAggregateStep
    {
        [JsonConstructor]
        public FilterSelectAggregateStep() { }
        protected FilterSelectAggregateStep(int order)
        {
            this.Order = order;
        }

        public FilterSelectAggregateStep(int order, FilterStep filter) : this(order)
        {
            this.Filter = filter;
        }
        public FilterSelectAggregateStep(int order, SelectStep select) : this(order)
        {
            this.Select = select;
        }
        public FilterSelectAggregateStep(int order, AggregateStep aggregate) : this(order)
        {
            this.Aggregate = aggregate;
        }

        public int Order { get; set; }
        public FilterStep Filter { get; set; }
        public SelectStep Select { get; set; }
        public AggregateStep Aggregate { get; set; }
    }
}