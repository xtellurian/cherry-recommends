using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterSetRecommendationStore : EFEnvironmentScopedEntityStoreBase<ParameterSetRecommendation>, IParameterSetRecommendationStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFParameterSetRecommendationStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.ParameterSetRecommendations)
        { }

        public async Task<ParameterSetRecommendation> GetRecommendationFromCorrelator(long correlatorId)
        {
            return await QuerySet
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
            return await QuerySet.AnyAsync(_ => _.RecommendationCorrelatorId == correlatorId);
        }

        public async Task<Paginated<ParameterSetRecommendation>> QueryForRecommender(int page, int? pageSize, long recommenderId)
        {
            int actualPageSize = pageSize ?? PageSize;
            var itemCount = await QuerySet.CountAsync(_ => _.RecommenderId == recommenderId);
            List<ParameterSetRecommendation> results;
            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(_ => _.RecommenderId == recommenderId)
                    .Include(_ => _.Customer)
                    .OrderByDescending(_ => _.Created)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * actualPageSize).Take(actualPageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<ParameterSetRecommendation>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / actualPageSize);
            return new Paginated<ParameterSetRecommendation>(results, pageCount, itemCount, page);
        }

        public async Task<IEnumerable<ParameterSetRecommendation>> RecommendationsSince(long recommenderId,
                                                                                        Customer customer,
                                                                                        DateTimeOffset since)
        {
            return await QuerySet
                .Where(_ => _.RecommenderId == recommenderId &&
                    _.TrackedUserId == customer.Id &&
                    _.Created > since)
                .ToListAsync();

        }

        public async Task<long> CountUniqueCustomers(long recommenderId)
        {
            if (await QuerySet.AnyAsync(_ => _.RecommenderId == recommenderId))
            {
                return await QuerySet
                    .Where(_ => _.RecommenderId == recommenderId)
                    .Select(_ => _.TrackedUserId)
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