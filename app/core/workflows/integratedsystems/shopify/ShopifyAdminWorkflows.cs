using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Integrations;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class ShopifyAdminWorkflows : ShopifyWorkflowBase, IShopifyAdminWorkflow
    {
        private readonly IIntegratedSystemWorkflow integratedSystemWorkflows;
        private readonly IEnvironmentProvider environmentProvider;

        public ShopifyAdminWorkflows(
            IDateTimeProvider dateTimeProvider,
            IStorageContext storageContext,
            IShopifyService shopifyService,
            IntegratedSystemStoreCollection systemStoreCollection,
            IOptions<ShopifyAppCredentials> creds,
            ILogger<ShopifyAdminWorkflows> logger,
            IIntegratedSystemWorkflow integratedSystemWorkflows,
            IEnvironmentProvider environmentProvider)
            : base(dateTimeProvider, storageContext, shopifyService, systemStoreCollection, creds, logger)
        {
            this.integratedSystemWorkflows = integratedSystemWorkflows;
            this.environmentProvider = environmentProvider;
        }

        public async Task<IntegratedSystem> Connect(string code, string shop, string webhookReceiverUrl, long? environmentId)
        {
            // Use the correct environment
            if (environmentId.HasValue)
            {
                environmentProvider.SetOverride(environmentId);
            }

            IntegratedSystem system = null!;
            // Read or create integrated system
            if (await systemStoreCollection.IntegratedSystemStore.ExistsFromCommonId(shop))
            {
                system = await systemStoreCollection.IntegratedSystemStore.ReadFromCommonId(shop);
                // Ensure disconnect the system before connecting to Shopify
                if (system.IntegrationStatus == IntegrationStatuses.OK)
                {
                    await Disconnect(system);
                }
            }
            else
            {
                system = new IntegratedSystem(shop, shop, IntegratedSystemTypes.Shopify)
                {
                    EnvironmentId = environmentId
                };
                system = await systemStoreCollection.IntegratedSystemStore.Create(system);
                await storageContext.SaveChanges();
            }

            try
            {
                // Fetch Shopify access token
                await base.Authorize(system, code, shop);
                // Create webhook receiver
                var webhookReceiver = await integratedSystemWorkflows.AddWebhookReceiver(system.Id, includeSharedSecret: false);
                // Create Shopify webhooks for event subscription
                var receiverUrl = webhookReceiverUrl.Replace("x-endpoint-id", webhookReceiver.EndpointId);
                string accessToken = GetAccessToken(system);
                // Create webhooks
                await shopifyService.CreateWebhook(shop, accessToken, receiverUrl, "app/uninstalled");
                await shopifyService.CreateWebhook(shop, accessToken, receiverUrl, "orders/paid");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Shopify connect failed. Reverting changes.");
                await base.UninstallApp(system, errorOnUninstall: false);
                await systemStoreCollection.IntegratedSystemStore.Remove(system.Id);
                await storageContext.SaveChanges();

                throw new WorkflowException("Shopify connect failed. Please retry the app installation.");
            }

            return system;
        }

        public async Task Disconnect(IntegratedSystem system)
        {
            SystemTypeGuard(system);

            if (system.IntegrationStatus == IntegrationStatuses.NotConfigured)
            {
                return;
            }

            string shopifyUrl = GetShopifyUrl(system);
            string accessToken = GetAccessToken(system);

            await base.UninstallApp(system, errorOnUninstall: false);

            system.ClearCache();
            system.CacheLastRefreshed = dateTimeProvider.Now;
            system.IntegrationStatus = IntegrationStatuses.NotConfigured;

            // Remove all webhook receivers
            await systemStoreCollection.IntegratedSystemStore.LoadMany(system, s => s.WebhookReceivers);
            system.WebhookReceivers.Clear();

            await systemStoreCollection.IntegratedSystemStore.Update(system);
            await storageContext.SaveChanges();
        }

        public async Task<ShopifyShop?> GetShopInformation(IntegratedSystem system)
        {
            SystemTypeGuard(system);

            if (system.IntegrationStatus == IntegrationStatuses.NotConfigured)
            {
                return null;
            }

            string shopifyUrl = GetShopifyUrl(system);
            string accessToken = GetAccessToken(system);
            return await shopifyService.GetShopInformation(shopifyUrl, accessToken);
        }
    }
}