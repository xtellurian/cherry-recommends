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
        : base(correlator, version)
        {
            Recommender = recommender;
            TrackedUser = trackedUser;
        }

        public TrackedUser TrackedUser { get; set; }
        public ParameterSetRecommender Recommender { get; set; }
    }
}