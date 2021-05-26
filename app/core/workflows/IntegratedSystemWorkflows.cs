using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class IntegratedSystemWorkflows
    {
        private List<string> PossibleSystems = new List<string>
        {
            "HUBSPOT",
            "CHARGEBEE"
        };

        private readonly IStorageContext storageContext;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore trackedUserSystemMapStore;
        private readonly ITrackedUserStore trackedUserStore;

        public IntegratedSystemWorkflows(IStorageContext storageContext,
                                         IIntegratedSystemStore integratedSystemStore,
                                         ITrackedUserSystemMapStore trackedUserSystemMapStore,
                                         ITrackedUserStore trackedUserStore)
        {
            this.storageContext = storageContext;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserSystemMapStore = trackedUserSystemMapStore;
            this.trackedUserStore = trackedUserStore;
        }

        public async Task<IntegratedSystem> CreateIntegratedSystem(string name, string systemType)
        {
            if (PossibleSystems.Contains(systemType))
            {
                var system = await integratedSystemStore.Create(new IntegratedSystem(name, systemType));
                await storageContext.SaveChanges();
                return system;
            }
            else
            {
                throw new ConfigurationException($"SystemType must be one of {string.Join(',', PossibleSystems)}");
            }
        }

        public async Task<TrackedUserSystemMap> LinkTrackedUserToSystem(string systemUserId, long integratedSystemId, string commonUserId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            var trackedUser = await trackedUserStore.ReadFromCommonUserId(commonUserId);
            var link = await trackedUserSystemMapStore.Create(new TrackedUserSystemMap(systemUserId, system, trackedUser));
            await storageContext.SaveChanges();
            return link;
        }
    }
}