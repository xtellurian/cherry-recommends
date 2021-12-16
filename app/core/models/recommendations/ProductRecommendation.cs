using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;


namespace SignalBox.Core.Recommendations
{
    public class ProductRecommendation : RecommendationEntity
    {
        protected ProductRecommendation()
        { }
#nullable enable
        public ProductRecommendation(
            ProductRecommender recommender,
            Customer customer,
            RecommendationCorrelator correlator,
            Product product,
            string? trigger)
         : base(correlator, RecommenderTypes.Product, trigger)
        {
            Recommender = recommender;
            RecommenderId = recommender.Id;
            Customer = customer;
            Product = product;
        }

#nullable enable
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProductRecommender? Recommender { get; set; }
        public Product Product { get; set; }
    }
}