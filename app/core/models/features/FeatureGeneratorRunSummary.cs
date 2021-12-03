namespace SignalBox.Core.Features.Generators
{
    public class FeatureGeneratorRunSummary
    {
        public FeatureGeneratorRunSummary()
        { }

        public FeatureGeneratorRunSummary(int totalWrites)
        {
            this.TotalWrites = totalWrites;
        }

        public bool? Enqueued { get; set; }
        public int? TotalWrites { get; set; }
        public int? MaxSubsetSize { get; set; }
    }
}