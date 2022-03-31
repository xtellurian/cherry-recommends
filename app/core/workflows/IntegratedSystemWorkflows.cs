using System;
using System.Threading.Tasks;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Core.Workflows
{
    public class IntegratedSystemWorkflows : IIntegratedSystemWorkflow, IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IntegratedSystemStoreCollection systemStoreCollection;
        private readonly ITrackedUserSystemMapStore trackedUserSystemMapStore;
        private readonly IWebhookReceiverStore webhookReceiverStore;
        private readonly IHasher hasher;
        private readonly ICustomerStore customerStore;

        public IntegratedSystemWorkflows(IStorageContext storageContext,
                                         IntegratedSystemStoreCollection systemStoreCollection,
                                         ITrackedUserSystemMapStore trackedUserSystemMapStore,
                                         IWebhookReceiverStore webhookReceiverStore,
                                         IHasher hasher,
                                         ICustomerStore customerStore)
        {
            this.storageContext = storageContext;
            this.systemStoreCollection = systemStoreCollection;
            this.trackedUserSystemMapStore = trackedUserSystemMapStore;
            this.webhookReceiverStore = webhookReceiverStore;
            this.hasher = hasher;
            this.customerStore = customerStore;
        }

        public async Task<IntegratedSystem> CreateIntegratedSystem(string name, string systemTypeName)
        {

            if (Enum.TryParse(systemTypeName, out IntegratedSystemTypes systemType))
            {
                if (systemType == IntegratedSystemTypes.Custom)
                {
                    var system = await systemStoreCollection.CustomIntegratedSystemStore.Create(new CustomIntegratedSystem($"New-{Guid.NewGuid()}".ToString(), name));
                    await storageContext.SaveChanges();
                    return system;
                }
                else
                {
                    var system = await systemStoreCollection.IntegratedSystemStore.Create(new IntegratedSystem($"New-{Guid.NewGuid()}".ToString(), name, systemType));
                    await storageContext.SaveChanges();
                    return system;
                }
            }
            else
            {
                throw new ConfigurationException($"SystemType must be one of IntegratedSystemTypes");
            }
        }

        public async Task<TrackedUserSystemMap> LinkTrackedUserToSystem(string systemUserId, long integratedSystemId, string commonUserId)
        {
            var system = await systemStoreCollection.IntegratedSystemStore.Read(integratedSystemId);
            var customer = await customerStore.ReadFromCommonId(commonUserId);
            var link = await trackedUserSystemMapStore.Create(new TrackedUserSystemMap(systemUserId, system, customer));
            await storageContext.SaveChanges();
            return link;
        }

        public async Task<WebhookReceiver> AddWebhookReceiver(long integratedSystemId, bool? includeSharedSecret)
        {
            var system = await systemStoreCollection.IntegratedSystemStore.Read(integratedSystemId);
            switch (system.SystemType)
            {
                case IntegratedSystemTypes.Segment:
                case IntegratedSystemTypes.Shopify:
                    break;
                default: throw new BadRequestException("Only Segment and Shopify webhooks are currently supported.");
            }
            var endpointId = hasher.Hash(System.Guid.NewGuid().ToBase64Encoded());
            var sharedSecret = (includeSharedSecret == true) ? hasher.Hash(System.Guid.NewGuid().ToBase64Encoded()) : null;
            var webhookReceiver = await webhookReceiverStore.Create(new WebhookReceiver(endpointId, system, sharedSecret));
            await storageContext.SaveChanges();
            return webhookReceiver;
        }
    }
}