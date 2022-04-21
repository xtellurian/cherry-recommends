using System;
using System.Threading.Tasks;
using SignalBox.Core.Integrations.Custom;
using SignalBox.Core.Integrations.Website;

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
                else if (systemType == IntegratedSystemTypes.Website)
                {
                    var system = await systemStoreCollection.WebsiteIntegratedSystemStore.Create(new WebsiteIntegratedSystem($"New-{Guid.NewGuid()}".ToString(), name));
                    await storageContext.SaveChanges();
                    return system;
                }
                else
                {
                    var system = await systemStoreCollection.IntegratedSystemStore.Create(new IntegratedSystem($"New-{Guid.NewGuid()}".ToString(), name, systemType));
                    await storageContext.SaveChanges();

                    if (system.SystemType == IntegratedSystemTypes.Segment)
                    {
                        await AddWebhookReceiver(system, true); // automatically add a webhook receiver for Sopify
                    }

                    return system;
                }
            }
            else
            {
                throw new ConfigurationException($"SystemType must be one of IntegratedSystemTypes");
            }
        }

        public async Task<TrackedUserSystemMap> LinkCustomerToSystem(string systemUserId, long integratedSystemId, string commonUserId)
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
            return await AddWebhookReceiver(system, includeSharedSecret);
        }

        private async Task<WebhookReceiver> AddWebhookReceiver(IntegratedSystem system, bool? includeSharedSecret)
        {
            switch (system.SystemType)
            {
                case IntegratedSystemTypes.Segment:
                case IntegratedSystemTypes.Shopify:
                    break;
                default: throw new BadRequestException("Only Segment and Shopify webhooks are currently supported.");
            }
            var endpointId = hasher.Hash(System.Guid.NewGuid().ToBase64Encoded());
            var sharedSecret = (includeSharedSecret == true) ? hasher.Hash(System.Guid.NewGuid().ToBase64Encoded()) : null;
            var webhookReceiver = new WebhookReceiver(endpointId, system, sharedSecret)
            {
                EnvironmentId = system.EnvironmentId
            };
            webhookReceiver = await webhookReceiverStore.Create(webhookReceiver);
            await storageContext.SaveChanges();
            return webhookReceiver;
        }

        public async Task SetIsDiscountCodeGenerator(long integratedSystemId, bool value)
        {
            var system = await systemStoreCollection.IntegratedSystemStore.Read(integratedSystemId);
            system.IsDiscountCodeGenerator = value;
            await systemStoreCollection.IntegratedSystemStore.Update(system);
            await storageContext.SaveChanges();
        }
    }
}