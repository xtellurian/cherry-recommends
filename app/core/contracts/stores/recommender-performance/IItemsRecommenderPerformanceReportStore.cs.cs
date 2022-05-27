using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IItemsRecommenderPerformanceReportStore : IEntityStore<ItemsRecommenderPerformanceReport>
    {
        Task<bool> HasReport(PromotionsCampaign campaign);
        Task<IEnumerable<ItemsRecommenderPerformanceReport>> ReadForRecommender(PromotionsCampaign campaign);
        Task<ItemsRecommenderPerformanceReport> ReadLatestForRecommender(PromotionsCampaign campaign);
    }
}