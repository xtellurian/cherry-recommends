namespace SignalBox.Core.Recommendations
{
    public class ParameterSetRecommendation : RecommendationEntity
    {
        protected ParameterSetRecommendation()
        { }

        public ParameterSetRecommendation(RecommendationCorrelator correlator, string version) : base(correlator)
        {
            this.Version = version;
        }

        public void SetInput<T>(T input) where T : IModelInput
        {
            this.ModelInput = Serialize(input);
            this.ModelInputType = typeof(T).FullName;
        }
        public T GetInput<T>() where T : IModelInput
        {
            return Deserialize<T>(this.ModelInput);
        }
        public void SetOutput<T>(T output) where T : IModelOutput
        {
            this.ModelOutput = Serialize(output);
            this.ModelOutputType = typeof(T).FullName;
        }
        public T GetOutput<T>() where T : IModelInput
        {
            return Deserialize<T>(this.ModelOutput);
        }

        public string Version { get; set; }
        public string ModelInput { get; set; } // JSON serialised
        public string ModelInputType { get; set; }
        public string ModelOutput { get; set; } // JSON serialised
        public string ModelOutputType { get; set; }
    }
}