using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryTrackedUserStore : InMemoryStore<TrackedUser>, ITrackedUserStore
    {
        private Task<bool> CommonUserIfExists(string commonUserId)
        {
            return Task.FromResult(store.Values.Any(_ => _.CommonUserId == commonUserId));
        }

        public Task<string> FindCommonId(long internalId)
        {
            if (store.ContainsKey(internalId))
            {
                return Task.FromResult(store[internalId].CommonUserId);
            }
            else
            {
                throw new EntityNotFoundException<TrackedUser>(internalId);
            }
        }

        public Task<long> GetInternalId(string commonUserId)
        {
            var result = store.Values.FirstOrDefault(_ => _.CommonUserId == commonUserId);
            return Task.FromResult(result.Id);
        }

        public Task<TrackedUser> ReadFromCommonId(string commonId)
        {
            var result = store.Values.FirstOrDefault(_ => _.CommonUserId == commonId);
            if (result == null)
            {
                throw new EntityNotFoundException<TrackedUser>(commonId);
            }

            return Task.FromResult(result);
        }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> commonUserIds)
        {
            var newUsers = new List<TrackedUser>();
            foreach (var commonId in commonUserIds)
            {
                if (!await ExistsFromCommonId(commonId))
                {
                    await this.Create(new TrackedUser(commonId));
                }
            }

            return newUsers;
        }

        public Task<bool> ExistsFromCommonId(string commonUserId)
        {
            return Task.FromResult(store.Values.Any(_ => _.CommonUserId == commonUserId));
        }

        public Task<TrackedUser> CreateIfNotExists(string commonId)
        {
            throw new NotImplementedException();
        }

        public Task<TrackedUser> ReadFromCommonId<TProperty>(string commonId, System.Linq.Expressions.Expression<Func<TrackedUser, TProperty>> include)
        {
            throw new NotImplementedException();
        }
    }
}