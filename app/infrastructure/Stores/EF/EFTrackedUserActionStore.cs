using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserActionStore : EFEntityStoreBase<TrackedUserAction>, ITrackedUserActionStore
    {
        public EFTrackedUserActionStore(SignalBoxDbContext context) : base(context, c => c.TrackedUserActions)
        { }

        public async Task<TrackedUserAction> ReadLatestAction(string commonUserId, string actionName)
        {
            var maxTimestamp = await Set
                .Where(_ => _.CommonUserId == commonUserId && _.ActionName == actionName)
                .Select(_ => _.Timestamp)
                .MaxAsync();

            return await Set
                .Where(_ => _.CommonUserId == commonUserId && _.ActionName == actionName && _.Timestamp == maxTimestamp)
                .FirstAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueActionNames(string commonUserId)
        {
            return await Set
              .Where(_ => _.CommonUserId == commonUserId)
              .Select(_ => _.ActionName)
              .Distinct()
              .ToListAsync();
        }
    }
}