using System;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class Offer : EnvironmentScopedEntity
    {
        protected Offer()
        { }

        public Offer(ItemsRecommendation recommendation)
        {
            RecommendationId = recommendation.Id;
            Recommendation = recommendation;
            State = OfferState.Created;
        }

        public long RecommendationId { get; set; }
        public long? RedeemedPromotionId { get; set; }
        public OfferState State { get; set; }
        public DateTimeOffset? RedeemedAt { get; set; }
        public double? GrossRevenue { get; set; }
        public int RedeemedCount { get; set; }

#nullable enable
        [JsonIgnore]
        public ItemsRecommendation Recommendation { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RecommendableItem? RedeemedPromotion { get; set; }
    }
}