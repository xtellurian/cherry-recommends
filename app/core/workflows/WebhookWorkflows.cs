using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Adapters.Segment;
using static SignalBox.Core.Workflows.CustomerEventsWorkflows;

namespace SignalBox.Core.Workflows
{
    public class WebhookWorkflows : IWorkflow
    {
        private readonly ILogger<WebhookWorkflows> logger;
        private readonly IWebhookReceiverStore receiverStore;
        private readonly CustomerEventsWorkflows eventsWorkflows;

        public WebhookWorkflows(ILogger<WebhookWorkflows> logger,
                                IWebhookReceiverStore receiverStore,
                                CustomerEventsWorkflows eventsWorkflows)
        {
            this.logger = logger;
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
            var trackedUserEventInput = JsonSerializer.Deserialize<SegmentModel>(webhookBody).ToTrackedUserEventInput(receiver.IntegratedSystem);

            var res = await eventsWorkflows.AddEvents(new List<CustomerEventInput> { trackedUserEventInput }, addToQueue: false);
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