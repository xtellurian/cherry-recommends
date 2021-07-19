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
                                          RecommendationCorrelator correlator,
                                          string version)
        : base(correlator, RecommenderTypes.ParameterSet, version)
        {
            Recommender = recommender;
            TrackedUser = trackedUser;
        }

#nullable enable
        public TrackedUser TrackedUser { get; set; }
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ParameterSetRecommender? Recommender { get; set; }
    }
}