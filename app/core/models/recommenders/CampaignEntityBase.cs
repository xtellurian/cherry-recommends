using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;

namespace SignalBox.Core.Campaigns
{
    public abstract class CampaignEntityBase : CommonEntity, ICampaign
    {
        public int MaxChannelCount => 3;

        protected CampaignEntityBase()
        { }

#nullable enable
        public CampaignEntityBase(string commonId, string? name, IEnumerable<CampaignArgument>? arguments, CampaignSettings? settings) : base(commonId, name)
        {
            this.ErrorHandling = settings is CampaignErrorHandling handling
                ? handling
                : new CampaignErrorHandling(settings);
            this.Settings = settings;
            this.OldArguments = arguments?.Select(_ => _.ToOldArgument())?.ToList() ?? new List<OldRecommenderArgument>();
            this.Arguments = arguments?.ToList() ?? new List<CampaignArgument>();
        }


        public bool ShouldThrowOnBadInput() => (this.Settings?.ThrowOnBadInput == true) || (this.ErrorHandling?.ThrowOnBadInput == true);
        public CampaignErrorHandling? ErrorHandling { get; set; }
        public CampaignSettings? Settings { get; set; }
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