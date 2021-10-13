using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
    public class ParameterSetRecommendation : RecommendationEntity
    {
        protected ParameterSetRecommendation()
        { }

        public ParameterSetRecommendation(ParameterSetRecommender recommender,
                                          TrackedUser trackedUser,
                                          RecommendationCorrelator correlator)
        : base(correlator, RecommenderTypes.ParameterSet)
        {
            Recommender = recommender;
            TrackedUser = trackedUser;
        }

#nullable enable
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ParameterSetRecommender? Recommender { get; set; }
    }
}