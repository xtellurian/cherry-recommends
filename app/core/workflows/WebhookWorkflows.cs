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
        private readonly IWebhookReceiverStore receiverStore;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly ITrackedUserEventStore trackedUserEventStore;
        private readonly IStorageContext storageContext;

        public WebhookWorkflows(ILogger<WebhookWorkflows> logger,
                                IWebhookReceiverStore receiverStore,
                                ITrackedUserStore trackedUserStore,
                                ITrackedUserEventStore trackedUserEventStore,
                                IStorageContext storageContext)
        {
            this.logger = logger;
            this.receiverStore = receiverStore;
            this.trackedUserStore = trackedUserStore;
            this.trackedUserEventStore = trackedUserEventStore;
            this.storageContext = storageContext;
        }

        public async Task ProcessWebhook(string endpointId, string webhookBody, string signature)
        {
            var receiver = await receiverStore.ReadFromEndpointId(endpointId);
            switch (receiver.IntegratedSystem.SystemType)
            {
                case IntegratedSystemTypes.Segment:
                    await ProcessSegmentWebhook(receiver, webhookBody, signature);
                    break;
                default:
                    throw new ArgumentException("Unprocessable Webhook type");

            }
        }

        private async Task<IEnumerable<TrackedUserEvent>> ProcessSegmentWebhook(WebhookReceiver receiver, string webhookBody, string signature)
        {
            // first check if the receiver has a shared secret, and if yes, validate the thing
            AssertSegmentSignatureValid(receiver, webhookBody, signature);
            var trackedUserEvent = JsonSerializer.Deserialize<SegmentModel>(webhookBody).ToTrackedUserEvent(receiver.IntegratedSystem);
            var trackedUser = await trackedUserStore.CreateIfNotExists(trackedUserEvent.CommonUserId);

            var results = await trackedUserEventStore.AddTrackedUserEvents(new List<TrackedUserEvent> { trackedUserEvent });
            await storageContext.SaveChanges();
            return results;
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