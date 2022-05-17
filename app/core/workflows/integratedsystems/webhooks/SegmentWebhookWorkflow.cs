using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SignalBox.Core.Adapters.Segment;

namespace SignalBox.Core.Workflows
{
    public class SegmentWebhookWorkflow : ISegmentWebhookWorkflow, IWorkflow
    {
        private readonly ILogger<SegmentWebhookWorkflow> logger;
        private readonly ICustomerEventsWorkflow eventsWorkflows;
        private readonly ITenantProvider tenantProvider;

        public SegmentWebhookWorkflow(
            ILogger<SegmentWebhookWorkflow> logger,
            ICustomerEventsWorkflow eventsWorkflows,
            ITenantProvider tenantProvider)
        {
            this.logger = logger;
            this.eventsWorkflows = eventsWorkflows;
            this.tenantProvider = tenantProvider;
        }

        public async Task<EventLoggingResponse> ProcessWebhookRequest(WebhookReceiver receiver, IEnumerable<KeyValuePair<string, StringValues>> headers, string body, string signature = null)
        {
            // first check if the receiver has a shared secret, and if yes, validate the thing
            AssertSegmentSignatureValid(receiver, body, signature);
            var segmentEvent = JsonSerializer.Deserialize<SegmentModel>(body);

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