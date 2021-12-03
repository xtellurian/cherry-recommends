using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserStore : EFCommonEntityStoreBase<TrackedUser>, ITrackedUserStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFTrackedUserStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, (c) => c.TrackedUsers)
        { }

        public async IAsyncEnumerable<TrackedUser> Iterate(int? limit = null)
        {
            int page = 1;
            bool hasMoreItems = true;
            while (hasMoreItems)
            {
                var itemCount = await QuerySet.CountAsync();
                List<TrackedUser> results;

                if (itemCount > 0) // check and let's see whether the query is worth running against the database
                {
                    results = await QuerySet
                        .OrderByDescending(_ => _.Created)
                        .Skip((page - 1) * PageSize).Take(PageSize)
                        .ToListAsync();
                }
                else
                {
                    results = new List<TrackedUser>();
                }
                var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
                var q = new Paginated<TrackedUser>(results, pageCount, itemCount, page);
                foreach (var item in q.Items)
                {
                    yield return item;
                }
                page++;
                hasMoreItems = q.Pagination.HasNextPage;
                if (limit != null && q.Pagination.PageNumber * PageSize > limit.Value)
                {
                    hasMoreItems = false;
                }
            }
        }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> commonIds)
        {
            var users = new List<TrackedUser>();
            foreach (var commonId in commonIds)
            {
                // Current behaviour is to not update the name.
                // this is because the name is auto-generated when recommenders are called
                // on a user that does not exist yet. 
                // Updating the name here would overwrite existing names that might be valid.
                users.Add(await this.CreateIfNotExists(commonId));
            }

            return users;
        }

        public async Task<TrackedUser> CreateIfNotExists(string commonId, string name = null)
        {
            if (!await this.QuerySet.AnyAsync(_ => _.CommonId == commonId))
            {
                return await this.Create(new TrackedUser(commonId, name));
            }
            else
            {
                return await this.ReadFromCommonId(commonId);
            }
        }

        public async Task<long> GetInternalId(string commonId)
        {
            var entity = await QuerySet.SingleAsync(_ => _.CommonId == commonId);
            return entity.Id;
        }

        public async Task<TrackedUser> ReadFromCommonUserId(string commonId)
        {
            return await QuerySet.SingleAsync(_ => _.CommonId == commonId);
        }
    }
}