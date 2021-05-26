using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserStore : EFEntityStoreBase<TrackedUser>, ITrackedUserStore
    {
        public EFTrackedUserStore(SignalBoxDbContext context) : base(context, (c) => c.TrackedUsers)
        { }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> commonUserIds)
        {
            var users = new List<TrackedUser>();
            foreach (var commonUserId in commonUserIds)
            {
                if (!await this.Set.AnyAsync(_ => _.CommonUserId == commonUserId))
                {
                    users.Add(await this.Create(new TrackedUser(commonUserId)));
                }
                else
                {
                    users.Add(await this.ReadFromCommonUserId(commonUserId));
                }
            }

            return users;
        }

        public async Task<bool> ExistsCommonUserId(string commonUserId)
        {
            return await this.Set.AnyAsync(_ => _.CommonUserId == commonUserId);
        }

        public async Task<string> GetCommonUserId(long internalId)
        {
            var entity = await Set.FindAsync(internalId);
            return entity.CommonUserId;
        }

        public async Task<long> GetInternalId(string commonUserId)
        {
            var entity = await Set.SingleAsync(_ => _.CommonUserId == commonUserId);
            return entity.Id;
        }

        public async Task<TrackedUser> ReadFromCommonUserId(string commonUserId)
        {
            return await Set.SingleAsync(_ => _.CommonUserId == commonUserId);
        }
    }
}