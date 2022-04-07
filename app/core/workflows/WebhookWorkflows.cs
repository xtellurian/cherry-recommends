using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace SignalBox.Core.Workflows
{
    public class WebhookWorkflows : IWorkflow
    {
        private readonly ILogger<WebhookWorkflows> logger;
        private readonly ITenantProvider tenantProvider;
        private readonly IWebhookReceiverStore receiverStore;
        private readonly ICustomerEventsWorkflow eventsWorkflows;
        private readonly ISegmentWebhookWorkflow segmentWebhookWorkflow;
        private readonly IShopifyWebhookWorkflow shopifyWebhookWorkflow;

        public WebhookWorkflows(
            ILogger<WebhookWorkflows> logger,
            ITenantProvider tenantProvider,
            IWebhookReceiverStore receiverStore,
            ICustomerEventsWorkflow eventsWorkflows,
            ISegmentWebhookWorkflow segmentWebhookWorkflow,
            IShopifyWebhookWorkflow shopifyWebhookWorkflow)
        {
            this.logger = logger;
            this.tenantProvider = tenantProvider;
            this.receiverStore = receiverStore;
            this.eventsWorkflows = eventsWorkflows;
            this.segmentWebhookWorkflow = segmentWebhookWorkflow;
            this.shopifyWebhookWorkflow = shopifyWebhookWorkflow;
        }

        public async Task ProcessWebhook(string endpointId, string webhookBody, IEnumerable<KeyValuePair<string, StringValues>> headers, string signature)
        {
            var receiver = await receiverStore.ReadFromEndpointId(endpointId);

            if (!receiver.Success)
            {
                return;
            }

            EventLoggingResponse eventLoggingResponse;

            switch (receiver.Entity.IntegratedSystem.SystemType)
            {
                case IntegratedSystemTypes.Segment:
                    eventLoggingResponse = await segmentWebhookWorkflow.ProcessWebhookRequest(receiver.Entity, headers, webhookBody, signature);
                    break;
                case IntegratedSystemTypes.Shopify:
                    eventLoggingResponse = await shopifyWebhookWorkflow.ProcessWebhookRequest(receiver.Entity, headers, webhookBody, signature);
                    break;
                default:
                    logger.LogCritical($"Unprocessable Webhook type: {receiver.Entity.IntegratedSystem.SystemType}");
                    throw new ArgumentException("Unprocessable Webhook type");
            }

            logger.LogInformation($"Processed {eventLoggingResponse.EventsProcessed} events");
        }
    }
}