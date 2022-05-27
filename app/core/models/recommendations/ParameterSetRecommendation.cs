using System.Text.Json.Serialization;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Recommendations
{
    public class ParameterSetRecommendation : RecommendationEntity
    {
        protected ParameterSetRecommendation()
        { }

        public ParameterSetRecommendation(ParameterSetCampaign recommender, RecommendingContext context)
        : base(context.Correlator, CampaignTypes.ParameterSet, context.Trigger)
        {
            Recommender = recommender;
            Customer = context.Customer;
        }

#nullable enable
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ParameterSetCampaign? Recommender { get; set; }
    }
}