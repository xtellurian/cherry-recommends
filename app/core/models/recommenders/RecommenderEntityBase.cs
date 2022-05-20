using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;

namespace SignalBox.Core.Recommenders
{
    public abstract class RecommenderEntityBase : CommonEntity, IRecommender
    {
        public int MaxChannelCount => 3;

        protected RecommenderEntityBase()
        { }

#nullable enable
        public RecommenderEntityBase(string commonId, string? name, IEnumerable<CampaignArgument>? arguments, RecommenderSettings? settings) : base(commonId, name)
        {
            this.ErrorHandling = settings is RecommenderErrorHandling handling
                ? handling
                : new RecommenderErrorHandling(settings);
            this.Settings = settings;
            this.OldArguments = arguments?.Select(_ => _.ToOldArgument())?.ToList() ?? new List<OldRecommenderArgument>();
            this.Arguments = arguments?.ToList() ?? new List<CampaignArgument>();
        }


        public bool ShouldThrowOnBadInput() => (this.Settings?.ThrowOnBadInput == true) || (this.ErrorHandling?.ThrowOnBadInput == true);
        public RecommenderErrorHandling? ErrorHandling { get; set; }
        public RecommenderSettings? Settings { get; set; }
        // todo old arguments are obsolete.
        // [Obsolete]
        public List<OldRecommenderArgument>? OldArguments { get; set; }
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
        [JsonIgnore]
        public ICollection<ChannelBase> Channels { get; set; } = new List<ChannelBase>();
        [JsonIgnore]
        public ICollection<CampaignArgument> Arguments { get; set; }
        [JsonIgnore]
        public ICollection<ArgumentRule> ArgumentRules { get; set; } = new List<ArgumentRule>();
    }
}