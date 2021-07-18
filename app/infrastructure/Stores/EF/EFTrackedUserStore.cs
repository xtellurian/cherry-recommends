using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserStore : EFCommonEntityStoreBase<TrackedUser>, ITrackedUserStore
    {
        public EFTrackedUserStore(SignalBoxDbContext context) : base(context, (c) => c.TrackedUsers)
        { }

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
            if (!await this.Set.AnyAsync(_ => _.CommonId == commonId))
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
            var entity = await Set.SingleAsync(_ => _.CommonId == commonId);
            return entity.Id;
        }

        public async Task<TrackedUser> ReadFromCommonUserId(string commonId)
        {
            return await Set.SingleAsync(_ => _.CommonId == commonId);
        }
    }
}