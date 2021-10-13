using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public class SimpleItemsRecommendationCache : SimpleRecommendationCache<ItemsRecommender, ItemsRecommendation>
    {
        public SimpleItemsRecommendationCache(IItemsRecommendationStore store, IDateTimeProvider dateTimeProvider)
        : base(store, dateTimeProvider)
        { }
    }
}