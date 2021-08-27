


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
        protected EFRecommenderStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector) : base(context, selector)
        { }

        public async Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(long id, int page)
        {
            var itemCount = await Set.Where(_ => _.Id == id).SelectMany(_ => _.RecommenderInvokationLogs).CountAsync();
            List<InvokationLogEntry> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(_ => _.Id == id)
                    .SelectMany(_ => _.RecommenderInvokationLogs)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<InvokationLogEntry>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<InvokationLogEntry>(results, pageCount, itemCount, page);
        }

        public abstract Task<Paginated<TrackedUserAction>> QueryAssociatedActions(T recommender, int page, bool revenueOnly);
    }
}