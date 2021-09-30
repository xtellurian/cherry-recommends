using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Web.Config;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReactConfigController : ControllerBase
    {
        private readonly IOptionsMonitor<Auth0ReactConfig> options;
        private readonly ITenantProvider tenantProvider;

        public ReactConfigController(IOptionsMonitor<Auth0ReactConfig> options, ITenantProvider tenantProvider)
        {
            this.options = options;
            this.tenantProvider = tenantProvider;
        }

        [HttpGet("auth0")]
        public Auth0ReactConfig GetConfiguration()
        {
            var config = options.CurrentValue;
            config.Scope = Core.Security.Scopes.AllScopes(config.Scope, tenantProvider.Current());
            return config;
        }
    }
}