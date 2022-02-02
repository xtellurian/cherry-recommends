using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFItemsRecommenderPerformanceReportStore : EFEntityStoreBase<ItemsRecommenderPerformanceReport>, IItemsRecommenderPerformanceReportStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFItemsRecommenderPerformanceReportStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.ItemsRecommenderPerformanceReports)
        { }

        public async Task<IEnumerable<ItemsRecommenderPerformanceReport>> ReadForRecommender(ItemsRecommender recommender)
        {
            if (!await HasReport(recommender))
            {
                throw new StorageException($"Recommender {recommender.Id} has no reports");
            }

            return await QuerySet
                .Where(_ => _.RecommenderId == recommender.Id)
                .OrderBy(_ => _.Id)
                .ToListAsync();
        }

        public async Task<ItemsRecommenderPerformanceReport> ReadLatestForRecommender(ItemsRecommender recommender)
        {
            if (!await HasReport(recommender))
            {
                throw new StorageException($"Recommender {recommender.Id} has no reports");
            }

            return await QuerySet
                .Where(_ => _.RecommenderId == recommender.Id)
                .OrderBy(_ => _.Id)
                .FirstAsync();
        }

        public async Task<bool> HasReport(ItemsRecommender recommender)
        {
            return await QuerySet.AnyAsync(_ => _.RecommenderId == recommender.Id);
        }
    }
}