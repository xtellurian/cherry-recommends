using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
    public class ProductRecommendation : RecommendationEntity
    {
        protected ProductRecommendation()
        { }

        public ProductRecommendation(ProductRecommender recommender, TrackedUser trackedUser, RecommendationCorrelator correlator, string version, Product product)
         : base(correlator, RecommenderTypes.Product, version)
        {
            Recommender = recommender;
            RecommenderId = recommender.Id;
            TrackedUser = trackedUser;
            Product = product;
        }

#nullable enable
        public TrackedUser TrackedUser { get; set; }
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProductRecommender? Recommender { get; set; }
        public Product Product { get; set; }
    }
}