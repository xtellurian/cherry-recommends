using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Serialization;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [ContentSecurityPolicy("frame-ancestors 'none'")] // https://shopify.dev/apps/store/security/iframe-protection
    [Route("api/integratedsystems/{id}/shopify")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShopifyController : SignalBoxControllerBase
    {
        private readonly ILogger<ShopifyController> logger;
        private readonly IShopifyAdminWorkflow shopifyAdminWorkflows;
        private readonly IIntegratedSystemStore store;
        private readonly ITenantProvider tenantProvider;
        private readonly IShopifyService shopifyService;

        public ShopifyController(ILogger<ShopifyController> logger,
                                 IShopifyAdminWorkflow shopifyAdminWorkflows,
                                 IIntegratedSystemStore store,
                                 ITenantProvider tenantProvider,
                                 IShopifyService shopifyService)
        {
            this.logger = logger;
            this.shopifyAdminWorkflows = shopifyAdminWorkflows;
            this.store = store;
            this.tenantProvider = tenantProvider;
            this.shopifyService = shopifyService;
        }

        /// <summary>Retrieve Shopify app information.</summary>
        [HttpGet("/api/shopifyappinfo")]
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

        /// <summary>Redirects to the Shopify app installation.</summary>
        [HttpGet("/api/shopify/install")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorizationUrl(string shop)
        {
            var queryString = Request.Query
                .ToDictionary(_ => _.Key, _ => _.Value.ToString());

            if (!await shopifyService.IsAuthenticRequest(queryString))
            {
                throw new SecurityException("Invalid Shopify request.");
            }

            if (Request.Headers.TryGetValue("Host", out var host))
            {
                var redirectUrl = $"{Request.Scheme}://{host}/_connect/shopify/callback";
                var state = string.Empty;
                var url = await shopifyAdminWorkflows.BuildAuthorizationUrl(shop, redirectUrl, state);

                return Redirect(url.ToString());
            }

            return BadRequest();
        }

        /// <summary>Generate Shopify app installation link.</summary>
        [HttpGet("/api/authorizeurl")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthorizeUrl(string shop, string redirectUrl)
        {
            var queryString = Request.Query
                .Where(_ => _.Key != "redirectUrl")
                .ToDictionary(_ => _.Key, _ => _.Value.ToString());

            if (!await shopifyService.IsAuthenticRequest(queryString))
            {
                throw new SecurityException("Invalid Shopify request.");
            }

            string state = ""; // Optional
            var uri = await shopifyAdminWorkflows.BuildAuthorizationUrl(shop, redirectUrl, state);

            return Ok(uri.ToString());
        }

        /// <summary>Establish Shopify connection by creating a new integrated system. Call this after a successful Shopify app installation.</summary>
        [HttpPost("/api/shopify/connect")]
        public async Task<IActionResult> Connect(ShopifyConnectDto dto)
        {
            var queryString = Request.Query
                .ToDictionary(_ => _.Key, _ => _.Value.ToString());

            if (!await shopifyService.IsAuthenticRequest(queryString))
            {
                return BadRequest("Invalid Shopify request.");
            }

            var tenant = tenantProvider.Current();
            string webhookReceiverUrl = Url.Link("AcceptWebhook", new
            {
                endpointId = "x-endpoint-id"
            });
            if (tenant != null)
            {
                webhookReceiverUrl += $"?x-tenant={tenant.Name}";
            }
            var system = await shopifyAdminWorkflows.Connect(dto.Code, dto.Shop, webhookReceiverUrl, dto.EnvironmentId);
            return Ok(system);
        }

        /// <summary>Disconnect Shopify connection. Uninstalls app and clears the access token.</summary>
        [HttpPost("Disconnect")]
        public async Task<IActionResult> Disconnect(long id)
        {
            var system = await store.Read(id);
            await shopifyAdminWorkflows.Disconnect(system);
            return Ok(id);
        }
    }
}