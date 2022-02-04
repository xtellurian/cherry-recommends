using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReactConfigController : ControllerBase
    {
        private readonly IOptionsMonitor<SegmentConfig> segmentOptions;
        private readonly IOptionsMonitor<Auth0ReactConfig> auth0Options;
        private readonly ITenantProvider tenantProvider;

        public ReactConfigController(IOptionsMonitor<Auth0ReactConfig> auth0Options, IOptionsMonitor<SegmentConfig> segmentOptions, ITenantProvider tenantProvider)
        {
            this.auth0Options = auth0Options;
            this.segmentOptions = segmentOptions;
            this.tenantProvider = tenantProvider;
        }

        [HttpGet("auth0")]
        public Auth0ReactConfig GetAuth0Configuration()
        {
            var config = auth0Options.CurrentValue;
            config.Scope = Core.Security.Scopes.AllScopes(config.Scope, tenantProvider.Current());
            return config;
        }

        [HttpGet]
        public ReactConfig GetReactConfig()
        {
            var segment = segmentOptions.CurrentValue;
            var config = new ReactConfig
            {
                Segment = segment
            };

            return config;
        }
    }
}