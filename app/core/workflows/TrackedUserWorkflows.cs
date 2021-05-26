
using System;
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

        public async Task<TrackedUser> CreateTrackedUser(string commonUserId, string? name = null, Dictionary<string, object>? properties = null)
        {
            if (await userStore.ExistsCommonUserId(commonUserId))
            {
                throw new System.ArgumentException($"Tracked User commonUserId={commonUserId} already exists.");
            }

            var trackedUser = await userStore.Create(new TrackedUser(commonUserId, name, new DynamicPropertyDictionary(properties)));
            await storageContext.SaveChanges();
            return trackedUser;
        }

        public async Task<TrackedUser> MergeTrackedUserProperties(string commonUserId, Dictionary<string, object> properties)
        {
            var trackedUser = await userStore.ReadFromCommonUserId(commonUserId);
            foreach (var kvp in properties)
            {
                trackedUser.Properties[kvp.Key] = kvp.Value;
            }
            await storageContext.SaveChanges();

            return trackedUser;
        }

        public async Task<IEnumerable<TrackedUser>> CreateOrUpdateMultipleTrackedUsers(
            IEnumerable<(string commonUserId, string? name, Dictionary<string, object>? properties)> newUsers)
        {
            var users = new List<TrackedUser>();
            foreach (var u in newUsers)
            {
                if (await userStore.ExistsCommonUserId(u.commonUserId))
                {
                    // then update
                    var user = await userStore.ReadFromCommonUserId(u.commonUserId);
                    if (u.properties != null)
                    {
                        foreach (var kvp in u.properties)
                        {
                            user.Properties[kvp.Key] = kvp.Value;
                        }
                    }
                    user.Name = u.name; // update the name too.
                    users.Add(user);
                }
                else
                {
                    var user = await userStore.Create(new TrackedUser(u.commonUserId, u.name, new DynamicPropertyDictionary(u.properties)));
                    users.Add(user);
                }
            }


            await storageContext.SaveChanges();
            return users;
        }
    }
}