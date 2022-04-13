using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SignalBox.Core;
using SignalBox.Core.Serialization;

namespace SignalBox.Functions
{
    public class ShopifyMandatoryWebhooks
    {
        private readonly ILogger<ShopifyMandatoryWebhooks> logger;
        private readonly IShopifyService shopifyService;

        // add all project dependencies to this ctor
        public ShopifyMandatoryWebhooks(ILogger<ShopifyMandatoryWebhooks> logger, IShopifyService shopifyService)
        {
            this.logger = logger;
            this.shopifyService = shopifyService;
        }

        [Function("Shopify_GDPRWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "webhooks/shopify/mandatory")] HttpRequestData req,
            FunctionContext executionContext)
        {
            logger.LogInformation("C# test boot triggered.");

            var headers = req.Headers
                .Select(_ => new KeyValuePair<string, StringValues>(_.Key, new StringValues(_.Value.ToArray())));
            var body = string.Empty;
            using (var reader = new StreamReader(req.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            if (!await shopifyService.IsAuthenticWebhook(headers, body))
            {
                logger.LogError("Invalid Shopify request.");
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var payloadInfo = Serializer.Deserialize<Dictionary<string, object>>(body);
            logger.LogInformation(Serializer.Serialize(payloadInfo));
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
