using System;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
    public abstract class RecommendationEntity : EnvironmentScopedEntity
    {
        protected RecommendationEntity()
        { }
#nullable enable
        public RecommendationEntity(RecommendationCorrelator correlator, RecommenderTypes recommenderType, string? trigger)
        {
            this.RecommendationCorrelator = correlator;
            this.RecommenderType = recommenderType;
            this.Trigger = trigger;
        }

        public RecommenderTypes? RecommenderType { get; set; } // nullable for backwards compat
        public long? TrackedUserId { get; set; }
        public Customer? TrackedUser => Customer;
        public Customer? Customer { get; set; }
        public long? TargetMetricId { get; set; }
        public Metric? TargetMetric { get; set; }

        public string? Trigger { get; set; }

        public long? RecommendationCorrelatorId { get; set; }
        [JsonIgnore]
        public RecommendationCorrelator RecommendationCorrelator { get; set; } // nullable for backwards compat

        public void SetInput<T>(T input) where T : IModelInput
        {
            this.ModelInput = Serialize(input);
            this.ModelInputType = typeof(T).FullName;
        }
        public T GetInput<T>() where T : class, IModelInput
        {
            return Deserialize<T>(this.ModelInput);
        }
        public void SetOutput<T>(T output) where T : IModelOutput
        {
            this.ModelOutput = Serialize(output);
            this.ModelOutputType = typeof(T).FullName;
        }
        public T GetOutput<T>() where T : class, IModelOutput
        {
            return Deserialize<T>(this.ModelOutput);
        }

        public string? ModelInput { get; set; } // JSON serialised
        public string? ModelInputType { get; set; }
        public string? ModelOutput { get; set; } // JSON serialised
        public string? ModelOutputType { get; set; }
        public bool IsFromCache { get; set; }
    }
}