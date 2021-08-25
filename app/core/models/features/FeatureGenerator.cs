namespace SignalBox.Core
{
    public enum FeatureGeneratorTypes
    {
        MonthsSinceEarliestEvent
    }

    public class FeatureGenerator : Entity
    {
        protected FeatureGenerator()
        { }
        public FeatureGenerator(Feature feature, FeatureGeneratorTypes generatorType)
        {
            this.Feature = feature;
            this.FeatureId = feature.Id;
            this.GeneratorType = generatorType;
        }

        public long FeatureId { get; set; }
        public Feature Feature { get; set; }
        public FeatureGeneratorTypes GeneratorType { get; set; }
    }
}