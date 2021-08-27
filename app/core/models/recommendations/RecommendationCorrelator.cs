using System.Collections.Generic;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
#nullable enable
    public class RecommendationCorrelator : Entity
    {
        protected RecommendationCorrelator()
        { }
        public RecommendationCorrelator(ParameterSetRecommender recommender)
        {
            this.ParameterSetRecommenderId = recommender.Id;
            this.ParameterSetRecommender = recommender;
            this.ModelRegistrationId = recommender.ModelRegistrationId;
            this.ModelRegistration = recommender.ModelRegistration;
        }
        public RecommendationCorrelator(ProductRecommender recommender)
        {
            this.ProductRecommenderId = recommender.Id;
            this.ProductRecommender = recommender;
            this.ModelRegistrationId = recommender.ModelRegistrationId;
            this.ModelRegistration = recommender.ModelRegistration;
        }

        public long? ParameterSetRecommenderId { get; set; }
        public ParameterSetRecommender? ParameterSetRecommender { get; set; }
        public long? ProductRecommenderId { get; set; }
        public ProductRecommender? ProductRecommender { get; set; }
        public ICollection<TrackedUserAction> TrackedUserActions { get; set; } = null!;
        public long? ModelRegistrationId { get; set; }
        public ModelRegistration? ModelRegistration { get; set; }
    }
}