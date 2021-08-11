
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
        private readonly ITrackedUserActionStore trackedUserActionStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public TrackedUserWorkflows(IStorageContext storageContext,
            ILogger<TrackedUserWorkflows> logger,
            ITrackedUserStore userStore,
            ITrackedUserEventStore eventStore,
            ITrackedUserSystemMapStore trackedUserSystemMapStore,
            IIntegratedSystemStore integratedSystemStore,
            ITrackedUserActionStore trackedUserActionStore,
            IDateTimeProvider dateTimeProvider)
        {
            this.storageContext = storageContext;
            this.logger = logger;
            this.userStore = userStore;
            this.eventStore = eventStore;
            this.trackedUserSystemMapStore = trackedUserSystemMapStore;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserActionStore = trackedUserActionStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<TrackedUser>> CreateIfNotExist(IEnumerable<string> commonUserIds)
        {
            var newUsers = await userStore.CreateIfNotExists(commonUserIds);
            foreach (var u in newUsers)
            {
                u.LastUpdated = dateTimeProvider.Now;
            }

            await storageContext.SaveChanges();
            return newUsers;
        }
        public async Task<TrackedUser> MergeUpdateProperties(TrackedUser trackedUser, IDictionary<string, object> properties, long? integratedSystemId = null, bool? saveOnComplete = true)
        {
            await userStore.LoadMany(trackedUser, _ => _.Actions);
            trackedUser.Actions ??= new List<TrackedUserAction>();
            var newProperties = new DynamicPropertyDictionary(properties);
            foreach (var a in trackedUser.ActionsFromChanges(newProperties, dateTimeProvider.Now, null, integratedSystemId))
            {
                trackedUser.Actions.Add(await trackedUserActionStore.Create(a));
            }

            trackedUser.Properties = newProperties;
            trackedUser.LastUpdated = dateTimeProvider.Now;

            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }
            return trackedUser;
        }

        public async Task<TrackedUser> CreateOrUpdateTrackedUser(string commonUserId,
                                                         string? name = null,
                                                         Dictionary<string, object>? properties = null,
                                                         long? integratedSystemId = null,
                                                         string? integratedSystemUserId = null,
                                                         bool saveOnComplete = true)
        {
            TrackedUser trackedUser;
            var actions = new List<TrackedUserAction>();

            if (await userStore.ExistsFromCommonId(commonUserId))
            {
                trackedUser = await userStore.ReadFromCommonId(commonUserId, _ => _.IntegratedSystemMaps);
                logger.LogInformation($"Updating user {trackedUser.Id}");
                if (!string.IsNullOrEmpty(name))
                {
                    trackedUser.Name = name;
                }
                if (properties != null && properties.Keys.Count > 0)
                {
                    trackedUser = await MergeUpdateProperties(trackedUser, properties, integratedSystemId, saveOnComplete: false);
                }
            }
            else
            {
                trackedUser = await userStore.Create(new TrackedUser(commonUserId, name, new DynamicPropertyDictionary(properties)));
                logger.LogInformation($"Created user {trackedUser.Id}");
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


            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }

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
                var user = await this.CreateOrUpdateTrackedUser(u.CommonUserId, u.Name, u.Properties, u.IntegratedSystemId, u.IntegratedSystemUserId, false);
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