using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterSetRecommendationStore : EFEntityStoreBase<ParameterSetRecommendation>, IParameterSetRecommendationStore
    {
        public EFParameterSetRecommendationStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.ParameterSetRecommendations)
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

        public async Task<Paginated<ParameterSetRecommendation>> QueryForRecommender(int page, long recommenderId)
        {
            var itemCount = await QuerySet.CountAsync(_ => _.RecommenderId == recommenderId);
            List<ParameterSetRecommendation> results;
            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(_ => _.RecommenderId == recommenderId)
                    .Include(_ => _.Customer)
                    .OrderByDescending(_ => _.Created)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<ParameterSetRecommendation>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
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
    }
}