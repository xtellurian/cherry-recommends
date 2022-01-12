using System.Collections.Generic;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
#nullable enable
    public class RecommendationCorrelator : Entity
    {
        protected RecommendationCorrelator()
        { }
        public RecommendationCorrelator(RecommenderEntityBase recommender)
        {
            this.RecommenderId = recommender.Id;
            this.Recommender = recommender;
            this.ModelRegistrationId = recommender.ModelRegistrationId;
            this.ModelRegistration = recommender.ModelRegistration;
        }

        public long? RecommenderId { get; set; }
        public RecommenderEntityBase? Recommender { get; set; }
        public long? ModelRegistrationId { get; set; }
        public ModelRegistration? ModelRegistration { get; set; }
    }
}