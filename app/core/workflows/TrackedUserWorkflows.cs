
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class TrackedUserWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly ILogger<TrackedUserWorkflows> logger;
        private readonly ITrackedUserStore userStore;
        private readonly ITrackedUserEventStore eventStore;
        private readonly ITrackedUserSystemMapStore trackedUserSystemMapStore;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public TrackedUserWorkflows(IStorageContext storageContext,
            ILogger<TrackedUserWorkflows> logger,
            ITrackedUserStore userStore,
            ITrackedUserEventStore eventStore,
            ITrackedUserSystemMapStore trackedUserSystemMapStore,
            IIntegratedSystemStore integratedSystemStore,
            IDateTimeProvider dateTimeProvider)
        {
            this.storageContext = storageContext;
            this.logger = logger;
            this.userStore = userStore;
            this.eventStore = eventStore;
            this.trackedUserSystemMapStore = trackedUserSystemMapStore;
            this.integratedSystemStore = integratedSystemStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<TrackedUser> CreateOrUpdateTrackedUser(string commonUserId,
                                                         string? name = null,
                                                         Dictionary<string, object>? properties = null,
                                                         long? integratedSystemId = null,
                                                         string? integratedSystemUserId = null)
        {
            TrackedUser trackedUser;
            logger.LogInformation("Creating single tracked user");
            if (await userStore.ExistsFromCommonId(commonUserId))
            {
                trackedUser = await userStore.ReadFromCommonId(commonUserId, _ => _.IntegratedSystemMaps);
                if (properties != null && properties.Keys.Count > 0)
                {
                    trackedUser.Properties = new DynamicPropertyDictionary(properties);
                }
            }
            else
            {
                trackedUser = await userStore.Create(new TrackedUser(commonUserId, name, new DynamicPropertyDictionary(properties)));
            }

            if (integratedSystemId.HasValue && !trackedUser.IntegratedSystemMaps.Any(_ => _.IntegratedSystemId == integratedSystemId))
            {
                logger.LogInformation($"Connecting user to integrated system: {integratedSystemId}");
                var integratedSystem = await integratedSystemStore.Read(integratedSystemId.Value);
                await trackedUserSystemMapStore.Create(new TrackedUserSystemMap(integratedSystemUserId, integratedSystem, trackedUser));
            }
            else
            {
                logger.LogWarning($"Not setting integratedSystemId for tracked user {trackedUser.Id}");
            }
            await storageContext.SaveChanges();
            return trackedUser;
        }

        public async Task<TrackedUser> MergeTrackedUserProperties(string commonUserId, Dictionary<string, object> properties)
        {
            var trackedUser = await userStore.ReadFromCommonId(commonUserId);
            foreach (var kvp in properties)
            {
                trackedUser.Properties[kvp.Key] = kvp.Value;
            }
            await storageContext.SaveChanges();

            return trackedUser;
        }

        public async Task<IEnumerable<TrackedUser>> CreateOrUpdateMultipleTrackedUsers(
            IEnumerable<CreateOrUpdateTrackedUserModel> newUsers)
        {
            // check for incoming duplicates
            var numDistinct = newUsers.Select(_ => _.CommonUserId).Distinct().Count();
            if (numDistinct < newUsers.Count())
            {
                throw new BadRequestException("All CommonUserId must be unique when batch creating or updating.");
            }
            var users = new List<TrackedUser>();
            foreach (var u in newUsers)
            {
                if (await userStore.ExistsFromCommonId(u.CommonUserId))
                {
                    // then update
                    TrackedUser user;
                    if (u.IntegratedSystemId != null)
                    {
                        user = await userStore.ReadFromCommonId(u.CommonUserId, _ => _.IntegratedSystemMaps);
                    }
                    else
                    {
                        user = await userStore.ReadFromCommonId(u.CommonUserId);
                    }
                    if (u.Properties != null)
                    {
                        foreach (var kvp in u.Properties)
                        {
                            user.Properties[kvp.Key] = kvp.Value;
                        }
                    }
                    user.Name = u.Name; // update the name too.
                    users.Add(user);
                    if (u.IntegratedSystemId.HasValue)
                    {
                        var integratedSystem = await integratedSystemStore.Read(u.IntegratedSystemId.Value);
                        var map = user.IntegratedSystemMaps.FirstOrDefault(_ => _.IntegratedSystem.Id == u.IntegratedSystemId);
                        if (map == null)
                        {
                            map = new TrackedUserSystemMap(u.IntegratedSystemUserId, integratedSystem, user);
                            map = await trackedUserSystemMapStore.Create(map);
                        }
                        else
                        {
                            map.UserId = u.IntegratedSystemUserId; // in that case ensure this value matches
                        }
                    }
                }
                else
                {
                    var user = await userStore.Create(new TrackedUser(u.CommonUserId, u.Name, u.Properties));
                    users.Add(user);
                }
            }


            await storageContext.SaveChanges();
            return users;
        }

        public struct CreateOrUpdateTrackedUserModel
        {
            public CreateOrUpdateTrackedUserModel(string commonUserId,
                                             string? name,
                                             Dictionary<string, object>? properties,
                                             long? integratedSystemId,
                                             string? integratedSystemUserId)
            {
                CommonUserId = commonUserId;
                Name = name;
                Properties = properties;
                IntegratedSystemId = integratedSystemId;
                IntegratedSystemUserId = integratedSystemUserId;
            }

            public string CommonUserId { get; set; }
            public string? Name { get; set; }
            public Dictionary<string, object>? Properties { get; set; }
            public long? IntegratedSystemId { get; set; }
            public string? IntegratedSystemUserId { get; set; }
        }
    }
}