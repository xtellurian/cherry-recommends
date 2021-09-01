using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFItemsRecommendationStore : EFEntityStoreBase<ItemsRecommendation>, IItemsRecommendationStore
    {
        public EFItemsRecommendationStore(SignalBoxDbContext context)
        : base(context, (c) => c.ItemsRecommendations)
        {
        }

        public async Task<ItemsRecommendation> GetRecommendationFromCorrelator(long correlatorId)
        {
            return await Set
                .Include(_ => _.Recommender)
                .ThenInclude(_=>_.ModelRegistration)
                .SingleAsync(_ => _.RecommendationCorrelatorId == correlatorId);
        }

        public async Task<bool> CorrelationExists(long? correlatorId)
        {
            if (correlatorId == null)
            {
                return false;
            }
            return await Set.AnyAsync(_ => _.RecommendationCorrelatorId == correlatorId);
        }

        public async Task<Paginated<ItemsRecommendation>> QueryForRecommender(int page, long recommenderId)
        {
            var itemCount = await Set.CountAsync(_ => _.RecommenderId == recommenderId);
            List<ItemsRecommendation> results;
            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(_ => _.RecommenderId == recommenderId)
                    .Include(_ => _.Items)
                    .Include(_ => _.TrackedUser)
                    .OrderByDescending(_ => _.Created)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<ItemsRecommendation>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<ItemsRecommendation>(results, pageCount, itemCount, page);
        }

    }
}