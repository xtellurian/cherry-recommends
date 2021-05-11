using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFTrackedUserStore : EFEntityStoreBase<TrackedUser>, ITrackedUserStore
    {
        public EFTrackedUserStore(SignalBoxDbContext context) : base(context, (c) => c.TrackedUsers)
        { }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> externalIds)
        {
            var users = new List<TrackedUser>();
            foreach (var externalId in externalIds)
            {
                if (!await this.Set.AnyAsync(_ => _.ExternalId == externalId))
                {
                    users.Add(await this.Create(new TrackedUser(externalId)));
                }
                else
                {
                    users.Add(await this.ReadFromExternalId(externalId));
                }
            }

            return users;
        }

        public async Task<bool> ExistsExternalId(string externalId)
        {
            return await this.Set.AnyAsync(_ => _.ExternalId == externalId);
        }

        public async Task<string> GetExternalId(long internalId)
        {
            var entity = await Set.FindAsync(internalId);
            return entity.ExternalId;
        }

        public async Task<long> GetInternalId(string externalId)
        {
            var entity = await Set.SingleAsync(_ => _.ExternalId == externalId);
            return entity.Id;
        }

        public async Task<TrackedUser> ReadFromExternalId(string externalId)
        {
            return await Set.SingleAsync(_ => _.ExternalId == externalId);
        }
    }
}