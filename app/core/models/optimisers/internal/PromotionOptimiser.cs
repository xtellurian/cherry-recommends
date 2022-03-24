using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Optimisers
{
#nullable enable
    public class PromotionOptimiser : EnvironmentScopedEntity
    {
        protected PromotionOptimiser()
        { }

        public PromotionOptimiser(ItemsRecommender recommender)
        {
            RecommenderId = recommender.Id;
            Recommender = recommender;
        }

        [JsonIgnore]
        public long RecommenderId { get; set; }
        [JsonIgnore]
        public ItemsRecommender Recommender { get; set; } = null!;
        /// <summary>
        /// These are configured as an Owned Entity collection in the database.
        /// </summary>
        public ICollection<PromotionOptimiserWeight> Weights { get; set; } = null!;
    }
}