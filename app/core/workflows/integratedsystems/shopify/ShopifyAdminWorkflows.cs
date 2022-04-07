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

        public ShopifyAdminWorkflows(
            IDateTimeProvider dateTimeProvider,
            IStorageContext storageContext,
            IShopifyService shopifyService,
            IntegratedSystemStoreCollection systemStoreCollection,
            IOptions<ShopifyAppCredentials> creds,
            ILogger<ShopifyAdminWorkflows> logger,
            IIntegratedSystemWorkflow integratedSystemWorkflows)
            : base(dateTimeProvider, storageContext, shopifyService, systemStoreCollection, creds, logger)
        {
            this.integratedSystemWorkflows = integratedSystemWorkflows;
        }

        public async Task Connect(IntegratedSystem system, string code, string shopifyUrl, string webhookReceiverUrl)
        {
            await base.Authorize(system, code, shopifyUrl);
            // Create webhook receiver
            var webhookReceiver = await integratedSystemWorkflows.AddWebhookReceiver(system.Id, includeSharedSecret: false);
            // Create Shopify webhooks for event subscription
            var receiverUrl = webhookReceiverUrl.Replace("x-endpoint-id", webhookReceiver.EndpointId);
            string accessToken = GetAccessToken(system);

            await shopifyService.CreateWebhook(shopifyUrl, accessToken, receiverUrl, "app/uninstalled");
            await shopifyService.CreateWebhook(shopifyUrl, accessToken, receiverUrl, "orders/paid");
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