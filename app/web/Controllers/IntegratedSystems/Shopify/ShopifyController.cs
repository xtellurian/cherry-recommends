using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Shopify;
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

        public ShopifyController(ILogger<ShopifyController> logger,
                                 IShopifyAdminWorkflow shopifyAdminWorkflows,
                                 IIntegratedSystemStore store)
        {
            this.logger = logger;
            this.shopifyAdminWorkflows = shopifyAdminWorkflows;
            this.store = store;
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
        public async Task<ShopifyShop> GetShopInformation(long id)
        {
            var system = await store.Read(id);
            return await shopifyAdminWorkflows.GetShopInformation(system);
        }

        /// <summary>Generate Shopify app installation link.</summary>
        [HttpGet("Install")]
        public async Task<string> GetAuthorizationUrl(string id, string shopifyUrl, string redirectUrl)
        {
            var uri = await shopifyAdminWorkflows.BuildAuthorizationUrl(shopifyUrl, redirectUrl, state: id);

            return uri.ToString();
        }

        /// <summary>Establish Shopify connection. Call this after a successful Shopify app installation.</summary>
        [HttpPost("Connect")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Connect(long id, ShopifyCode dto)
        {
            var system = await store.Read(id);
            await shopifyAdminWorkflows.Connect(system, dto.Code, dto.ShopifyUrl);
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