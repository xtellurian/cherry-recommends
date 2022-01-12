using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFRecommendableItemStore : EFCommonEntityStoreBase<RecommendableItem>, IRecommendableItemStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFRecommendableItemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, (c) => c.RecommendableItems)
        { }

        public async Task<Paginated<RecommendableItem>> QueryForRecommender(long recommenderId, int page)
        {
            var itemCount = await context.ItemsRecommenders
                .Where(_ => _.Id == recommenderId)
                .SelectMany(_ => _.Items)
                .Distinct()
                .CountAsync();

            List<RecommendableItem> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await context.ItemsRecommenders
                    .Where(_ => _.Id == recommenderId)
                    .SelectMany(_ => _.Items)
                    .Distinct()
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<RecommendableItem>();
            }

            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<RecommendableItem>(results, pageCount, itemCount, page);
        }
    }
}