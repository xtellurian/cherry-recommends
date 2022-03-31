using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Core.Workflows
{
    public class ShopifyWebhookWorkflow : IShopifyWebhookWorkflow, IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IShopifyService shopifyService;
        private readonly ILogger<ShopifyWebhookWorkflow> logger;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ICustomerEventsWorkflow eventsWorkflows;
        private readonly ITenantProvider tenantProvider;
        private readonly IShopifyAdminWorkflow shopifyAdminWorkflow;

        public ShopifyWebhookWorkflow(
            IStorageContext storageContext,
            IShopifyService shopifyService,
            ILogger<ShopifyWebhookWorkflow> logger,
            IIntegratedSystemStore integratedSystemStore,
            ICustomerEventsWorkflow eventsWorkflows,
            ITenantProvider tenantProvider,
            IShopifyAdminWorkflow shopifyAdminWorkflow)
        {
            this.storageContext = storageContext;
            this.shopifyService = shopifyService;
            this.logger = logger;
            this.integratedSystemStore = integratedSystemStore;
            this.eventsWorkflows = eventsWorkflows;
            this.tenantProvider = tenantProvider;
            this.shopifyAdminWorkflow = shopifyAdminWorkflow;
        }

        public async Task<EventLoggingResponse> ProcessWebhookRequest(WebhookReceiver receiver, IEnumerable<KeyValuePair<string, StringValues>> headers, string body, string signature = null)
        {
            if (!await shopifyService.IsAuthenticWebhook(headers, body))
            {
                throw new SecurityException($"Received a non-authentic Shopify webhook request for receiver id={receiver.Id}");
            }
            var webhookId = string.Join(";", headers.First(_ => _.Key == "X-Shopify-Webhook-Id").Value);
            var topic = string.Join(";", headers.First(_ => _.Key == "X-Shopify-Topic").Value);

            EventLoggingResponse eventLoggingResponse = null;

            switch (topic)
            {
                case "app/uninstalled":
                    eventLoggingResponse = await OnAppUninstalled(receiver.IntegratedSystem, body);
                    break;
                case "orders/paid":
                    eventLoggingResponse = await OnOrdersPayment(receiver.IntegratedSystem, body, webhookId, topic);
                    break;
                default:
                    throw new WorkflowException($"Unsupported Shopify topic: {topic} for receiver id={receiver.Id}");
            }

            return eventLoggingResponse;
        }

        private async Task<EventLoggingResponse> OnAppUninstalled(IntegratedSystem system, string body)
        {
            var shopifyEvent = JsonSerializer.Deserialize<ShopifyShop>(body);
            try
            {
                await integratedSystemStore.Remove(system.Id);
                await storageContext.SaveChanges();
                logger.LogInformation("Shopify app uninstalled. Integrated system id={integratedSystemId} removed", system.Id);
            }
            catch (Exception)
            {
                await shopifyAdminWorkflow.Disconnect(system);
                logger.LogInformation("Shopify app uninstalled. Integrated system id={integratedSystemId} was not removed due to constraints; system disconnected.", system.Id);
            }
            var res = await eventsWorkflows.Ingest(new List<CustomerEventInput> { });

            return res;
        }

        private async Task<EventLoggingResponse> OnOrdersPayment(IntegratedSystem system, string body, string webhookId, string topic)
        {
            var shopifyEvent = JsonSerializer.Deserialize<ShopifyOrder>(body);
            var customerEventInput = shopifyEvent.ToCustomerEventInput(webhookId, topic, tenantProvider, system);
            var res = await eventsWorkflows.Ingest(new List<CustomerEventInput> { customerEventInput });

            return res;
        }
    }
}