using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Adapters.Segment;

namespace SignalBox.Core.Workflows
{
    public class WebhookWorkflows : IWorkflow
    {
        private readonly ILogger<WebhookWorkflows> logger;
        private readonly ITenantProvider tenantProvider;
        private readonly IWebhookReceiverStore receiverStore;
        private readonly ICustomerEventsWorkflow eventsWorkflows;

        public WebhookWorkflows(
            ILogger<WebhookWorkflows> logger,
            ITenantProvider tenantProvider,
            IWebhookReceiverStore receiverStore,
            ICustomerEventsWorkflow eventsWorkflows)
        {
            this.logger = logger;
            this.tenantProvider = tenantProvider;
            this.receiverStore = receiverStore;
            this.eventsWorkflows = eventsWorkflows;
        }

        public async Task ProcessWebhook(string endpointId, string webhookBody, string signature)
        {
            var receiver = await receiverStore.ReadFromEndpointId(endpointId);
            EventLoggingResponse eventLoggingResponse;
            switch (receiver.IntegratedSystem.SystemType)
            {
                case IntegratedSystemTypes.Segment:
                    eventLoggingResponse = await ProcessSegmentWebhook(receiver, webhookBody, signature);
                    break;
                default:
                    logger.LogCritical($"Unprocessable Webhook type: {receiver.IntegratedSystem.SystemType}");
                    throw new ArgumentException("Unprocessable Webhook type");
            }

            logger.LogInformation($"Processed {eventLoggingResponse.EventsProcessed} events");
        }

        private async Task<EventLoggingResponse> ProcessSegmentWebhook(WebhookReceiver receiver, string webhookBody, string signature)
        {
            // first check if the receiver has a shared secret, and if yes, validate the thing
            AssertSegmentSignatureValid(receiver, webhookBody, signature);
            var segmentEvent = JsonSerializer.Deserialize<SegmentModel>(webhookBody);

            var trackedUserEventInput = segmentEvent.ToCustomerEventInput(tenantProvider, receiver.IntegratedSystem);
            var res = await eventsWorkflows.Ingest(new List<CustomerEventInput> { trackedUserEventInput });
            return res;
        }

        private void AssertSegmentSignatureValid(WebhookReceiver receiver, string webhookBody, string signature)
        {
            if (!string.IsNullOrEmpty(receiver.SharedSecret))
            {
                string digest;
                // then validate the request with the signature
                using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(receiver.SharedSecret)))
                {
                    var hash = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(webhookBody));
                    digest = hash.ToHexString();
                }
                if (!string.Equals(digest, signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityException("Webhook digest does not match computed signature");
                }
            }
            else
            {
                logger.LogWarning($"Webhook receiver id={receiver.Id} is not using a shared secret");
            }
        }
    }
}