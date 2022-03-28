using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Integrations;

namespace SignalBox.Core.Workflows
{
    public class ShopifyAdminWorkflows : ShopifyWorkflowBase, IShopifyAdminWorkflow
    {

        public ShopifyAdminWorkflows(
            IDateTimeProvider dateTimeProvider,
            IStorageContext storageContext,
            IShopifyService shopifyService,
            IntegratedSystemStoreCollection systemStoreCollection,
            IOptions<ShopifyAppCredentials> creds,
            ILogger<ShopifyAdminWorkflows> logger)
            : base(dateTimeProvider, storageContext, shopifyService, systemStoreCollection, creds, logger)
        { }

        public async Task Connect(IntegratedSystem system, string code, string shopifyUrl)
        {
            await base.Authorize(system, code, shopifyUrl);

            // Create Shopify webhooks for event subscription
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

            await systemStoreCollection.IntegratedSystemStore.Update(system);
            await storageContext.SaveChanges();
        }

        public async Task<ShopifyShop> GetShopInformation(IntegratedSystem system)
        {
            string shopifyUrl = GetShopifyUrl(system);
            string accessToken = GetAccessToken(system);
            return await shopifyService.GetShopInformation(shopifyUrl, accessToken);
        }
    }
}