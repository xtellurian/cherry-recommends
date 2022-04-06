using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    [AllowTenantNull]
    public class ReactConfigController : ControllerBase
    {
        private readonly IOptionsMonitor<SegmentConfig> segmentOptions;
        private readonly IOptionsMonitor<LaunchDarklyConfig> launchDarklyOptions;
        private readonly IOptionsMonitor<Auth0ReactConfig> auth0Options;
        private readonly IOptionsMonitor<Hosting> hostingOptions;
        private readonly IOptionsMonitor<HotjarConfig> hotjarOptions;

        private readonly ITenantProvider tenantProvider;

        public ReactConfigController(IOptionsMonitor<Auth0ReactConfig> auth0Options,
                                     IOptionsMonitor<Hosting> hostingOptions,
                                     IOptionsMonitor<SegmentConfig> segmentOptions,
                                     IOptionsMonitor<LaunchDarklyConfig> launchDarklyOptions,
                                     ITenantProvider tenantProvider,
                                     IOptionsMonitor<HotjarConfig> hotjarOptions)
        {
            this.auth0Options = auth0Options;
            this.hostingOptions = hostingOptions;
            this.segmentOptions = segmentOptions;
            this.launchDarklyOptions = launchDarklyOptions;
            this.hotjarOptions = hotjarOptions;
            this.tenantProvider = tenantProvider;
        }
        private Auth0ReactConfig CreateAuth0Config()
        {
            var config = auth0Options.CurrentValue;
            // we need to get all the tenant scopes that the user has access to.

            config.Scope = Core.Security.Scopes.AllScopes(config.Scope, tenantProvider.Current());
            return config;
        }

        [HttpGet("auth0")]
        public Auth0ReactConfig GetAuth0Configuration()
        {
            return CreateAuth0Config();
        }

        [HttpGet("hosting")]
        [AllowAnonymous]
        [AllowTenantNull]
        public Hosting GetHosting()
        {
            return hostingOptions.CurrentValue;
        }

        [HttpGet]
        public ReactConfig GetReactConfig()
        {
            var config = new ReactConfig
            {
                Auth0 = CreateAuth0Config(),
                Segment = segmentOptions.CurrentValue,
                Hotjar = hotjarOptions.CurrentValue,
                LaunchDarkly = launchDarklyOptions.CurrentValue,
            };

            return config;
        }
    }
}
