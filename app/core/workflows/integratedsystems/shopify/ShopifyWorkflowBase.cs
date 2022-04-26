using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Integrations;

namespace SignalBox.Core.Workflows
{
    public abstract class ShopifyWorkflowBase : IShopifyWorkflowBase
    {
        protected readonly IDateTimeProvider dateTimeProvider;
        protected readonly IStorageContext storageContext;
        protected readonly IShopifyService shopifyService;
        protected readonly IntegratedSystemStoreCollection systemStoreCollection;
        protected readonly ShopifyAppCredentials creds;
        protected readonly ILogger<ShopifyWorkflowBase> logger;

        public ShopifyWorkflowBase(
            IDateTimeProvider dateTimeProvider,
            IStorageContext storageContext,
            IShopifyService shopifyService,
            IntegratedSystemStoreCollection systemStoreCollection,
            IOptions<ShopifyAppCredentials> creds,
            ILogger<ShopifyWorkflowBase> logger)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.storageContext = storageContext;
            this.shopifyService = shopifyService;
            this.systemStoreCollection = systemStoreCollection;
            this.logger = logger;
            this.creds = creds.Value;
        }

        public Task<ShopifyAppCredentials> GetShopifyCredentials()
        {
            if (string.IsNullOrEmpty(creds.ApiKey))
            {
                throw new WorkflowException("Shopify integration not configured correctly.");
            }
            return Task.FromResult(creds);
        }

        public Task<Uri> BuildAuthorizationUrl(string shopifyUrl, string redirectUrl, string state)
        {
            return shopifyService.BuildAuthorizationUrl(shopifyUrl, redirectUrl, state);
        }

        public async Task Authorize(IntegratedSystem system, string code, string shopifyUrl)
        {
            SystemTypeGuard(system);

            if (!await shopifyService.IsValidShopDomainAsync(shopifyUrl))
            {
                throw new BadRequestException("Invalid Shopify URL.");
            }

            string accessToken = await shopifyService.Authorize(code, shopifyUrl);
            var storeAccess = new ShopifyStoreCredentials(shopifyUrl, accessToken);

            system.SetCache(new ShopifyCache(storeAccess));
            system.CacheLastRefreshed = dateTimeProvider.Now;

            await systemStoreCollection.IntegratedSystemStore.Update(system);
            await storageContext.SaveChanges();
        }

        public async Task UninstallApp(IntegratedSystem system, bool errorOnUninstall = true)
        {
            SystemTypeGuard(system);

            if (string.IsNullOrEmpty(system.Cache))
            {
                return;
            }

            string shopifyUrl = GetShopifyUrl(system);
            string accessToken = GetAccessToken(system);
            if (!await shopifyService.UninstallApp(shopifyUrl, accessToken))
            {
                var exception = new WorkflowException($"Shopify app uninstallation failed for {shopifyUrl}");
                if (errorOnUninstall)
                {
                    throw exception;
                }
                else
                {
                    logger.LogWarning(exception, "An error occured when attempting to uninstall the Shopify app. Exception not thrown.");
                }
            }
            else
            {
                logger.LogInformation("Shopify app uninstallation successful for integrated system {id} - {shopifyUrl}", system.Id, shopifyUrl);
            }
        }

        public string GetShopifyUrl(IntegratedSystem system)
        {
            var cache = system.GetCache<ShopifyCache>();
            var shopifyUrl = cache.StoreCredentials.ShopifyUrl;

            if (string.IsNullOrEmpty(shopifyUrl))
            {
                throw new WorkflowException("Shopify integration not configured correctly.");
            }

            return shopifyUrl;
        }

        public string GetAccessToken(IntegratedSystem system)
        {
            var cache = system.GetCache<ShopifyCache>();
            var accessToken = cache.StoreCredentials.AccessToken;

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new WorkflowException("Shopify integration not configured correctly.");
            }

            return accessToken;
        }

        protected void SystemTypeGuard(IntegratedSystem system)
        {
            if (system.SystemType != IntegratedSystemTypes.Shopify)
            {
                throw new BadRequestException("Only Shopify system is allowed.");
            }
        }
    }
}