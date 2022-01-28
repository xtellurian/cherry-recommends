namespace SignalBox.Core.Metrics
{

    public enum AggregationTypes
    {
        Sum,
        Mean
    }

    public class AggregateStep
    {
        public AggregateStep()
        { }
        public AggregationTypes AggregationType { get; set; }
    }
}