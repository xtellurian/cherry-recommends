using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace SignalBox.Functions
{
    public class ShopifyMandatoryWebhooks
    {
        private readonly ILogger<ShopifyMandatoryWebhooks> logger;

        // add all project dependencies to this ctor
        public ShopifyMandatoryWebhooks(ILogger<ShopifyMandatoryWebhooks> logger)
        {
            this.logger = logger;
        }

        [Function("Shopify_GDPRWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "webhooks/shopify/mandatory")] HttpRequestData req,
            FunctionContext executionContext)
        {
            logger.LogInformation("C# test boot triggered.");
            var payloadInfo = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(req.Body);
            logger.LogInformation(JsonSerializer.Serialize(payloadInfo));
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
