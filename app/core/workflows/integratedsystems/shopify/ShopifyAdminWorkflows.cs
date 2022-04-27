using System;
using System.Linq;
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
        private readonly ShopifyBilling billingInfo;
        private readonly IIntegratedSystemWorkflow integratedSystemWorkflows;
        private readonly IEnvironmentProvider environmentProvider;

        public ShopifyAdminWorkflows(
            IDateTimeProvider dateTimeProvider,
            IStorageContext storageContext,
            IShopifyService shopifyService,
            IntegratedSystemStoreCollection systemStoreCollection,
            IOptions<ShopifyAppCredentials> creds,
            IOptions<ShopifyBilling> billingInfo,
            ILogger<ShopifyAdminWorkflows> logger,
            IIntegratedSystemWorkflow integratedSystemWorkflows,
            IEnvironmentProvider environmentProvider)
            : base(dateTimeProvider, storageContext, shopifyService, systemStoreCollection, creds, logger)
        {
            this.billingInfo = billingInfo.Value;
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

            IntegratedSystem system;
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
                // Set status to OK if there's no billing
                if (billingInfo.Skip)
                {
                    system.IntegrationStatus = IntegrationStatuses.OK;
                    await systemStoreCollection.IntegratedSystemStore.Update(system);
                    await systemStoreCollection.IntegratedSystemStore.Context.SaveChanges();
                }
                // Create webhook receiver
                var webhookReceiver = await integratedSystemWorkflows.AddWebhookReceiver(system.Id, includeSharedSecret: false);
                // Create Shopify webhooks for event subscription
                var receiverUrl = webhookReceiverUrl.Replace("x-endpoint-id", webhookReceiver.EndpointId);
                string accessToken = GetAccessToken(system);
                // Create webhooks
                await shopifyService.CreateWebhook(shop, accessToken, receiverUrl, "APP_UNINSTALLED");
                await shopifyService.CreateWebhook(shop, accessToken, receiverUrl, "ORDERS_PAID");
                await shopifyService.CreateWebhook(shop, accessToken, receiverUrl, "APP_SUBSCRIPTIONS_UPDATE");
                logger.LogInformation("Created webhooks for shop {shop}", shop);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Shopify connect failed. Reverting changes.");
                await base.UninstallApp(system, errorOnUninstall: false);

                throw new WorkflowException("Shopify connect failed. Please retry the app installation.");
            }

            return system;
        }

        public async Task Disconnect(IntegratedSystem system)
        {
            SystemTypeGuard(system);

            if (string.IsNullOrEmpty(system.Cache))
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

        public async Task<ShopifyRecurringCharge?> ChargeBilling(IntegratedSystem system, string returnUrl)
        {
            SystemTypeGuard(system);

            if (billingInfo.Skip)
            {
                return null;
            }

            string shop = GetShopifyUrl(system);
            string accessToken = GetAccessToken(system);

            var charges = await shopifyService.ListRecurringCharges(shop, accessToken);

            // Charge already exists
            if (charges.Any(_ => _.Name == billingInfo.Name && _.Status.ToLower() == "active"))
            {
                return null;
            }

            // Name should be unique per app charge
            var charge = new ShopifyRecurringCharge(billingInfo.Name, billingInfo.Price, billingInfo.Test, billingInfo.TrialDays)
            {
                ReturnUrl = returnUrl
            };
            charge = await shopifyService.CreateRecurringCharge(shop, accessToken, charge);
            logger.LogInformation("Created recurring app charge for shop {shop}", shop);

            return charge;
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