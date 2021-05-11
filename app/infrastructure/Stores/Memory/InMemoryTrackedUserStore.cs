using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryTrackedUserStore : InMemoryStore<TrackedUser>, ITrackedUserStore
    {
        private Task<bool> ExternalIdExists(string externalId)
        {
            return Task.FromResult(store.Values.Any(_ => _.ExternalId == externalId));
        }

        public Task<string> GetExternalId(long internalId)
        {
            if (store.ContainsKey(internalId))
            {
                return Task.FromResult(store[internalId].ExternalId);
            }
            else
            {
                throw new EntityNotFoundException(typeof(TrackedUser), internalId);
            }
        }

        public Task<long> GetInternalId(string externalId)
        {
            var result = store.Values.FirstOrDefault(_ => _.ExternalId == externalId);
            return Task.FromResult(result.Id);
        }

        public Task<TrackedUser> ReadFromExternalId(string externalId)
        {
            var result = store.Values.FirstOrDefault(_ => _.ExternalId == externalId);
            if (result == null)
            {
                throw new EntityNotFoundException($"Tracked User with External ID {externalId}");
            }

            return Task.FromResult(result);
        }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> externalIds)
        {
            var newUsers = new List<TrackedUser>();
            foreach (var id in externalIds)
            {
                if (!await ExternalIdExists(id))
                {
                    await this.Create(new TrackedUser(id));
                }
            }

            return newUsers;
        }

        public Task<bool> ExistsExternalId(string externalId)
        {
            return Task.FromResult(store.Values.Any(_ => _.ExternalId == externalId));
        }
    }
}