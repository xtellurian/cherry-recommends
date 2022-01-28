namespace SignalBox.Core.Metrics.Generators
{
    public class MetricGeneratorRunSummary
    {
        public MetricGeneratorRunSummary()
        { }

        public MetricGeneratorRunSummary(int totalWrites)
        {
            this.TotalWrites = totalWrites;
        }

        public bool? Enqueued { get; set; }
        public int? TotalWrites { get; set; }
        public int? MaxSubsetSize { get; set; }
    }
}