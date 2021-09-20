using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTouchpointStore : EFCommonEntityStoreBase<Touchpoint>, ITouchpointStore
    {
        public EFTouchpointStore(SignalBoxDbContext context, IEnvironmentService environmentService)
        : base(context, environmentService, c => c.Touchpoints)
        {
        }

        public async Task<Paginated<TrackedUser>> QueryTrackedUsers(int page, long touchpointId)
        {
            var itemCount = await Set
                    .Where(_ => _.Id == touchpointId)
                    .SelectMany(_ => _.TrackedUserTouchpoints.Select(_ => _.TrackedUser)).CountAsync();

            List<TrackedUser> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(_ => _.Id == touchpointId)
                    .SelectMany(_ => _.TrackedUserTouchpoints.Select(_ => _.TrackedUser))
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