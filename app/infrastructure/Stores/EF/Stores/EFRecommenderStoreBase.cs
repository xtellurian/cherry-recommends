using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFRecommenderStoreBase<T> : EFCommonEntityStoreBase<T>, IRecommenderStore<T> where T : RecommenderEntityBase
    {
        protected override bool IsEnvironmentScoped => true;
        protected EFRecommenderStoreBase(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(contextProvider, environmentService, selector)
        { }

        public async Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(IPaginate paginate, long id)
        {
            var pageSize = paginate.PageSize ?? DefaultPageSize;
            var itemCount = await QuerySet
                .Where(_ => _.Id == id)
                .SelectMany(_ => _.RecommenderInvokationLogs)
                .CountAsync();
            List<InvokationLogEntry> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(_ => _.Id == id)
                    .SelectMany(_ => _.RecommenderInvokationLogs)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((paginate.SafePage - 1) * pageSize).Take(pageSize)
                    .Include(_ => _.Customer)
                    .Include(_ => _.Business)
                    .ToListAsync();
            }
            else
            {
                results = new List<InvokationLogEntry>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<InvokationLogEntry>(results, pageCount, itemCount, paginate.SafePage);
        }
    }
}