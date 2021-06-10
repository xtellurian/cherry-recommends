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
            trackedUser = await context.TrackedUsers
               .Include(_ => _.TrackedUserTouchpoints)
               .ThenInclude(_ => _.Touchpoint)
               .FirstAsync(_ => _.Id == trackedUser.Id);

            var existing = trackedUser.TrackedUserTouchpoints
                .Where(_ => _.Touchpoint == touchpoint).ToList();
            return existing
                .Select(_ => _.Version)
                .DefaultIfEmpty(0)
                .Max();
        }

        public async Task<IEnumerable<Touchpoint>> GetTouchpointsFor(TrackedUser trackedUser)
        {
            trackedUser = await context.TrackedUsers
                .Include(_ => _.TrackedUserTouchpoints)
                .ThenInclude(_ => _.Touchpoint)
                .FirstAsync(_ => _.Id == trackedUser.Id);

            return trackedUser.TrackedUserTouchpoints.Select(_ => _.Touchpoint).ToList();
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
    }
}