using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IItemsRecommenderPerformanceReportStore : IEntityStore<ItemsRecommenderPerformanceReport>
    {
        Task<bool> HasReport(ItemsRecommender recommender);
        Task<IEnumerable<ItemsRecommenderPerformanceReport>> ReadForRecommender(ItemsRecommender recommender);
        Task<ItemsRecommenderPerformanceReport> ReadLatestForRecommender(ItemsRecommender recommender);
    }
}