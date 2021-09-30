using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFFeatureStore : EFCommonEntityStoreBase<Feature>, IFeatureStore
    {
        public EFFeatureStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService) 
        : base(contextProvider, environmentService, c => c.Features)
        { }

        public async Task<Paginated<TrackedUser>> QueryTrackedUsers(int page, long featureId)
        {
            var itemCount = await Set
                    .Where(_ => _.Id == featureId)
                    .SelectMany(_ => _.HistoricTrackedUserFeatures.Select(_ => _.TrackedUser))
                    .Distinct()
                    .CountAsync();

            List<TrackedUser> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(_ => _.Id == featureId)
                    .SelectMany(_ => _.HistoricTrackedUserFeatures.Select(_ => _.TrackedUser))
                    .OrderByDescending(_ => _.LastUpdated)
                    .Distinct()
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<TrackedUser>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<TrackedUser>(results, pageCount, itemCount, page);
        }
    }
}