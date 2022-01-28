using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;

namespace SignalBox.Core.Recommenders
{
    public abstract class RecommenderEntityBase : CommonEntity, IRecommender
    {
        protected RecommenderEntityBase()
        { }

#nullable enable
        public RecommenderEntityBase(string commonId, string? name, IEnumerable<RecommenderArgument>? arguments, RecommenderSettings? settings) : base(commonId, name)
        {
            this.ErrorHandling = settings is RecommenderErrorHandling
                ? (RecommenderErrorHandling)settings
                : new RecommenderErrorHandling(settings);
            this.Settings = settings;
            this.Arguments = arguments?.ToList() ?? new List<RecommenderArgument>();
        }


        public bool ShouldThrowOnBadInput() => (this.Settings?.ThrowOnBadInput == true) || (this.ErrorHandling?.ThrowOnBadInput == true);
        public RecommenderErrorHandling? ErrorHandling { get; set; }
        public RecommenderSettings? Settings { get; set; }
        public List<RecommenderArgument>? Arguments { get; set; }
        public TriggerCollection? TriggerCollection { get; set; }

        public long? ModelRegistrationId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ModelRegistration? ModelRegistration { get; set; }

        [JsonIgnore]
        public ICollection<Metric> LearningFeatures { get; set; } = null!; // TODO: - change this to LearningMetrics

        [JsonIgnore]
        public ICollection<InvokationLogEntry> RecommenderInvokationLogs { get; set; } = null!;
        [JsonIgnore]
        public ICollection<RecommendationCorrelator> RecommendationCorrelators { get; set; } = null!;

        [JsonIgnore]
        public ICollection<RecommendationDestinationBase> RecommendationDestinations { get; set; } = null!;
    }
}