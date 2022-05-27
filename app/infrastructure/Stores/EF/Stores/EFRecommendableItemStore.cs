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

        public async Task<Paginated<RecommendableItem>> QueryForRecommender(IPaginate paginate, long recommenderId)
        {
            var pageSize = paginate.PageSize ?? DefaultPageSize;
            var itemCount = await context.PromotionsCampaigns
                .Where(_ => _.Id == recommenderId)
                .SelectMany(_ => _.Items)
                .Distinct()
                .CountAsync();

            List<RecommendableItem> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await context.PromotionsCampaigns
                    .Where(_ => _.Id == recommenderId)
                    .SelectMany(_ => _.Items)
                    .Distinct()
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((paginate.SafePage - 1) * pageSize).Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<RecommendableItem>();
            }

            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<RecommendableItem>(results, pageCount, itemCount, paginate.SafePage);
        }
    }
}