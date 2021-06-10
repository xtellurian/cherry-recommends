using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class IntegratedSystemWorkflows
    {
        private readonly IStorageContext storageContext;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore trackedUserSystemMapStore;
        private readonly IWebhookReceiverStore webhookReceiverStore;
        private readonly IHasher hasher;
        private readonly ITrackedUserStore trackedUserStore;

        public IntegratedSystemWorkflows(IStorageContext storageContext,
                                         IIntegratedSystemStore integratedSystemStore,
                                         ITrackedUserSystemMapStore trackedUserSystemMapStore,
                                         IWebhookReceiverStore webhookReceiverStore,
                                         IHasher hasher,
                                         ITrackedUserStore trackedUserStore)
        {
            this.storageContext = storageContext;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserSystemMapStore = trackedUserSystemMapStore;
            this.webhookReceiverStore = webhookReceiverStore;
            this.hasher = hasher;
            this.trackedUserStore = trackedUserStore;
        }

        public async Task<IntegratedSystem> CreateIntegratedSystem(string name, string systemTypeName)
        {

            if (Enum.TryParse(systemTypeName, out IntegratedSystemTypes systemType))
            {
                var system = await integratedSystemStore.Create(new IntegratedSystem(name, systemType));
                await storageContext.SaveChanges();
                return system;
            }
            else
            {
                throw new ConfigurationException($"SystemType must be one of IntegratedSystemTypes");
            }
        }

        public async Task<TrackedUserSystemMap> LinkTrackedUserToSystem(string systemUserId, long integratedSystemId, string commonUserId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            var trackedUser = await trackedUserStore.ReadFromCommonId(commonUserId);
            var link = await trackedUserSystemMapStore.Create(new TrackedUserSystemMap(systemUserId, system, trackedUser));
            await storageContext.SaveChanges();
            return link;
        }

        public async Task<WebhookReceiver> AddWebhookReceiver(long integratedSystemId, bool? includeSharedSecret)
        {
            var integratedSystem = await integratedSystemStore.Read(integratedSystemId);
            if (integratedSystem.SystemType != IntegratedSystemTypes.Segment)
            {
                throw new BadRequestException("Only Segment webhooks are currently supported.");
            }
            var endpointId = hasher.Hash(System.Guid.NewGuid().ToBase64Encoded());
            var sharedSecret = (includeSharedSecret == true) ? hasher.Hash(System.Guid.NewGuid().ToBase64Encoded()) : null;
            var webhookReceiver = await webhookReceiverStore.Create(new WebhookReceiver(endpointId, integratedSystem, sharedSecret));
            await storageContext.SaveChanges();
            return webhookReceiver;
        }
    }
}