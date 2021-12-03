namespace SignalBox.Core.Features
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