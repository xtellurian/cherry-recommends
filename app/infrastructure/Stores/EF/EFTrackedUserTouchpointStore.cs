using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserTouchpointStore : EFEntityStoreBase<TrackedUserTouchpoint>, ITrackedUserTouchpointStore
    {
        public EFTrackedUserTouchpointStore(SignalBoxDbContext context) : base(context, c => c.TrackedUserTouchpoints)
        { }

        public async Task<int> CurrentMaximumTouchpointVersion(TrackedUser trackedUser, Touchpoint touchpoint)
        {
            // MaxAsync() w/ nullable int avoids 'Sequence contains no elements' error.
            return await context.TrackedUsers
               .Where(_ => _.Id == trackedUser.Id)
               .SelectMany(_ => _.TrackedUserTouchpoints)
               .Where(_ => _.TouchpointId == touchpoint.Id)
               .MaxAsync(_ => (int?)_.Version) ?? 0;
        }

        public async Task<IEnumerable<Touchpoint>> GetTouchpointsFor(TrackedUser trackedUser)
        {
            return await context.TrackedUsers
                .Where(_ => _.Id == trackedUser.Id)
                .SelectMany(_ => _.TrackedUserTouchpoints)
                .Select(_ => _.Touchpoint)
                .Distinct() // ensure we only return each touchpoint once.
                .ToListAsync();
        }

        public async Task<TrackedUserTouchpoint> ReadTouchpoint(TrackedUser trackedUser, Touchpoint touchpoint, int? version = null)
        {
            version ??= await CurrentMaximumTouchpointVersion(trackedUser, touchpoint);
            trackedUser = await context.TrackedUsers
                .Include(_ => _.TrackedUserTouchpoints)
                .ThenInclude(_ => _.Touchpoint)
                .FirstAsync(_ => _.Id == trackedUser.Id);

            return trackedUser.TrackedUserTouchpoints.First(_ => _.Touchpoint == touchpoint && _.Version == version.Value);

        }

        public async Task<bool> TouchpointExists(TrackedUser trackedUser, Touchpoint touchpoint, int? version = null)
        {
            version ??= await CurrentMaximumTouchpointVersion(trackedUser, touchpoint);
            trackedUser = await context.TrackedUsers
                .Include(_ => _.TrackedUserTouchpoints)
                .ThenInclude(_ => _.Touchpoint)
                .FirstAsync(_ => _.Id == trackedUser.Id);

            return trackedUser.TrackedUserTouchpoints.Any(_ => _.Touchpoint == touchpoint && _.Version == version.Value);
        }
    }
}