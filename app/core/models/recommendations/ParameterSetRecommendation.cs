using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
    public class ParameterSetRecommendation : RecommendationEntity
    {
        protected ParameterSetRecommendation()
        { }

        public ParameterSetRecommendation(ParameterSetRecommender recommender, RecommendingContext context)
        : base(context.Correlator, RecommenderTypes.ParameterSet, context.Trigger)
        {
            Recommender = recommender;
            TrackedUser = context.TrackedUser;
        }

#nullable enable
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ParameterSetRecommender? Recommender { get; set; }
    }
}