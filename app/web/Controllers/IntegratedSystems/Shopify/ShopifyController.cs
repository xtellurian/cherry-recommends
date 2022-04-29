using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
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

            var tenant = tenantProvider.Current();
            var redirectUrl = $"{Request.Scheme}://{Request.Host}/_connect/shopify/callback";
            var state = string.Empty;
            if (tenant != null)
            {
                state = tenant.Name;
            }
            var url = await shopifyAdminWorkflows.BuildAuthorizationUrl(shop, redirectUrl, state);

            return Redirect(url.ToString());
        }

        /// <summary>Establish Shopify connection by creating a new integrated system. Call this after a successful Shopify app installation.</summary>
        [HttpPost("/api/shopify/connect")]
        public async Task<IActionResult> Connect(ShopifyConnectDto dto)
        {
            var response = new ShopifyConnectSuccessDto();

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

            var returnUrl = $"{Request.Scheme}://{Request.Host}/_connect/shopify/callback?x-id={system.Id}";
            if (tenant != null)
            {
                returnUrl += $"&x-tenant={tenant.Name}";
            }
            if (dto.EnvironmentId.HasValue)
            {
                returnUrl += $"&x-environment={dto.EnvironmentId.Value}";
            }
            var charge = await shopifyAdminWorkflows.ChargeBilling(system, returnUrl);

            response.IntegratedSystem = system;
            if (charge != null && !string.IsNullOrEmpty(charge.ConfirmationUrl))
            {
                response.ChargeConfirmationUrl = charge.ConfirmationUrl;
            }

            return Ok(response);
        }

        /// <summary>Disconnect Shopify connection. Uninstalls app and clears the access token.</summary>
        [HttpPost("Disconnect")]
        public async Task<IActionResult> Disconnect(long id)
        {
            var system = await store.Read(id);
            await shopifyAdminWorkflows.UninstallApp(system, errorOnUninstall: false);
            await shopifyAdminWorkflows.Disconnect(system);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IntegratedSystem> Get(long id)
        {
            var system = await store.Read(id);
            return system;
        }
    }
}