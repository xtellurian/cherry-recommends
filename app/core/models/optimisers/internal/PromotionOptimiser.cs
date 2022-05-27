using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Optimisers
{
#nullable enable
    public class PromotionOptimiser : EnvironmentScopedEntity
    {
        protected PromotionOptimiser()
        { }

        public PromotionOptimiser(PromotionsCampaign campaign)
        {
            RecommenderId = campaign.Id;
            Recommender = campaign;
        }

        public long RecommenderId { get; set; }
        [JsonIgnore]
        public PromotionsCampaign Recommender { get; set; } = null!;
        /// <summary>
        /// These are configured as an Owned Entity collection in the database.
        /// </summary>
        public ICollection<PromotionOptimiserWeight> Weights { get; set; } = null!;
    }
}