using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFItemsRecommendationStore : EFEnvironmentScopedEntityStoreBase<ItemsRecommendation>, IItemsRecommendationStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFItemsRecommendationStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.ItemsRecommendations)
        { }

        public async Task<ItemsRecommendation> GetRecommendationFromCorrelator(long correlatorId)
        {
            return await Set
                .Include(_ => _.Recommender)
                .ThenInclude(_ => _.ModelRegistration)
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

        public async Task<Paginated<ItemsRecommendation>> QueryForRecommender(IPaginate paginate, long recommenderId)
        {
            var pageSize = paginate.PageSize ?? DefaultPageSize;
            var itemCount = await Set.CountAsync(_ => _.RecommenderId == recommenderId);
            List<ItemsRecommendation> results;
            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(_ => _.RecommenderId == recommenderId)
                    .Include(_ => _.Items)
                    .Include(_ => _.Customer)
                    .OrderByDescending(_ => _.Created)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((paginate.SafePage - 1) * pageSize).Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<ItemsRecommendation>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<ItemsRecommendation>(results, pageCount, itemCount, paginate.SafePage);
        }

        public async Task<IEnumerable<ItemsRecommendation>> RecommendationsSince(long recommenderId,
                                                                                 Customer customer,
                                                                                 DateTimeOffset since)
        {
            return await QuerySet
                 .Where(_ => _.RecommenderId == recommenderId &&
                     _.CustomerId == customer.Id &&
                     _.Created > since)
                 .ToListAsync();
        }

        public async Task<long> CountUniqueCustomers(long recommenderId)
        {
            if (await QuerySet.AnyAsync(_ => _.RecommenderId == recommenderId))
            {
                return await QuerySet
                    .Where(_ => _.RecommenderId == recommenderId)
                    .Select(_ => _.CustomerId)
                    .Distinct()
                    .CountAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<long> CountRecommendations(long recommenderId)
        {
            if (await QuerySet.AnyAsync(_ => _.RecommenderId == recommenderId))
            {
                return await QuerySet
                   .CountAsync(_ => _.RecommenderId == recommenderId);
            }
            else
            {
                return 0;
            }
        }
    }
}