
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class TrackedUserWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly ITrackedUserStore userStore;
        private readonly ITrackedUserEventStore eventStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public TrackedUserWorkflows(IStorageContext storageContext,
            ITrackedUserStore userStore,
            ITrackedUserEventStore eventStore,
            IDateTimeProvider dateTimeProvider)
        {
            this.storageContext = storageContext;
            this.userStore = userStore;
            this.eventStore = eventStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<TrackedUser> CreateTrackedUser(string externalId, string? name = null)
        {
            if (await userStore.ExistsExternalId(externalId))
            {
                throw new System.ArgumentException($"Tracked User externalId={externalId} already exists.");
            }

            var trackedUser = await userStore.Create(new TrackedUser(externalId, name));
            await storageContext.SaveChanges();
            return trackedUser;
        }

        public async Task<IEnumerable<TrackedUser>> CreateMultipleTrackedUsers(
            IEnumerable<(string externalId, string? name)> newUsers,
            IEnumerable<(string externalId, string? key, string? logicalValue, double? numericValue)>? newEvents)
        {
            var users = new List<TrackedUser>();
            foreach (var u in newUsers)
            {
                var user = await userStore.Create(new TrackedUser(u.externalId, u.name));
                users.Add(user);
            }

            if (newEvents != null)
            {
                var trackedEvents = newEvents.Where(e => e.key != null).Select(e =>
                      new TrackedUserEvent(e.externalId, dateTimeProvider.Now, e.key!, e.logicalValue, e.numericValue));
                await eventStore.AddTrackedUserEvents(trackedEvents);
            }

            await storageContext.SaveChanges();
            return users;
        }
    }
}