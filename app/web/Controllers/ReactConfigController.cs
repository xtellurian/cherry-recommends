using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalBox.Web.Config;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ReactConfigController : ControllerBase
    {
        private readonly IOptionsMonitor<Auth0ReactConfig> options;

        public ReactConfigController(IOptionsMonitor<Auth0ReactConfig> options)
        {
            this.options = options;
        }

        [HttpGet("auth0")]
        public Auth0ReactConfig GetConfiguration()
        {
            return options.CurrentValue;
        }
    }
}