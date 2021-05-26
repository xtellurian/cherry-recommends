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

        public Task<string> GetCommonUserId(long internalId)
        {
            if (store.ContainsKey(internalId))
            {
                return Task.FromResult(store[internalId].CommonUserId);
            }
            else
            {
                throw new EntityNotFoundException(typeof(TrackedUser), internalId);
            }
        }

        public Task<long> GetInternalId(string commonUserId)
        {
            var result = store.Values.FirstOrDefault(_ => _.CommonUserId == commonUserId);
            return Task.FromResult(result.Id);
        }

        public Task<TrackedUser> ReadFromCommonUserId(string commonUserId)
        {
            var result = store.Values.FirstOrDefault(_ => _.CommonUserId == commonUserId);
            if (result == null)
            {
                throw new EntityNotFoundException($"Tracked User with Common User ID {commonUserId}");
            }

            return Task.FromResult(result);
        }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> commonUserIds)
        {
            var newUsers = new List<TrackedUser>();
            foreach (var id in commonUserIds)
            {
                if (!await ExistsCommonUserId(id))
                {
                    await this.Create(new TrackedUser(id));
                }
            }

            return newUsers;
        }

        public Task<bool> ExistsCommonUserId(string commonUserId)
        {
            return Task.FromResult(store.Values.Any(_ => _.CommonUserId == commonUserId));
        }
    }
}