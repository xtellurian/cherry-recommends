using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFMetricStore : EFCommonEntityStoreBase<Metric>, IMetricStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFMetricStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, c => c.Metrics)
        { }

        public async Task<Paginated<Customer>> QueryCustomers(int page, long metricId)
        {
            var itemCount = await Set
                    .Where(_ => _.Id == metricId)
                    .SelectMany(_ => _.HistoricTrackedUserFeatures.Select(_ => _.TrackedUser))
                    .Distinct()
                    .CountAsync();

            List<Customer> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(_ => _.Id == metricId)
                    .SelectMany(_ => _.HistoricTrackedUserFeatures.Select(_ => _.TrackedUser))
                    .OrderByDescending(_ => _.LastUpdated)
                    .Distinct()
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<Customer>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<Customer>(results, pageCount, itemCount, page);
        }
    }
}