using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Serialization;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/integratedsystems/{id}/shopify")]
    public class ShopifyController : SignalBoxControllerBase
    {
        private readonly ILogger<ShopifyController> logger;
        private readonly IShopifyAdminWorkflow shopifyAdminWorkflows;
        private readonly IIntegratedSystemStore store;
        private readonly ITenantProvider tenantProvider;

        public ShopifyController(ILogger<ShopifyController> logger,
                                 IShopifyAdminWorkflow shopifyAdminWorkflows,
                                 IIntegratedSystemStore store,
                                 ITenantProvider tenantProvider)
        {
            this.logger = logger;
            this.shopifyAdminWorkflows = shopifyAdminWorkflows;
            this.store = store;
            this.tenantProvider = tenantProvider;
        }

        /// <summary>Retrieve Shopify app information.</summary>
        [HttpGet("/api/shopifyappinfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ShopifyAppInformation> GetAppInformation()
        {
            var creds = await shopifyAdminWorkflows.GetShopifyCredentials();
            return new ShopifyAppInformation(creds.ApiKey, creds.Scopes);
        }

        /// <summary>Retrieve Shopify shop information.</summary>
        [HttpGet("ShopInformation")]
        public async Task<IActionResult> GetShopInformation(long id)
        {
            var system = await store.Read(id);
            var shop = await shopifyAdminWorkflows.GetShopInformation(system);

            if (shop == null)
            {
                return NoContent();
            }

            return Ok(shop);
        }

        /// <summary>Generate Shopify app installation link.</summary>
        [HttpGet("Install")]
        public async Task<string> GetAuthorizationUrl(string id, string shopifyUrl, string redirectUrl)
        {
            var tenant = tenantProvider.Current();
            string state = Serializer.Serialize(new { id, tenant = tenant?.Name }, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var uri = await shopifyAdminWorkflows.BuildAuthorizationUrl(shopifyUrl, redirectUrl, state);

            return uri.ToString();
        }

        /// <summary>Establish Shopify connection. Call this after a successful Shopify app installation.</summary>
        [HttpPost("Connect")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Connect(long id, ShopifyCode dto)
        {
            var tenant = tenantProvider.Current();
            var system = await store.Read(id);
            string webhookReceiverUrl = Url.Link("AcceptWebhook", new
            {
                endpointId = "x-endpoint-id"
            });
            if (tenant != null)
            {
                webhookReceiverUrl += $"?x-tenant={tenant.Name}";
            }
            await shopifyAdminWorkflows.Connect(system, dto.Code, dto.ShopifyUrl, webhookReceiverUrl);
            return Ok(id);
        }

        /// <summary>Disconnect Shopify connection. Uninstalls app and clears the access token.</summary>
        [HttpPost("Disconnect")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Disconnect(long id)
        {
            var system = await store.Read(id);
            await shopifyAdminWorkflows.Disconnect(system);
            return Ok(id);
        }
    }
}