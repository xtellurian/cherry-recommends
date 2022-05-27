using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure
{
    public class SimpleItemsRecommendationCache : SimpleRecommendationCache<PromotionsCampaign, ItemsRecommendation>
    {
        public SimpleItemsRecommendationCache(IItemsRecommendationStore store, IDateTimeProvider dateTimeProvider)
        : base(store, dateTimeProvider)
        { }
    }
}