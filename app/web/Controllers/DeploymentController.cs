using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DeploymentController : ControllerBase
    {
        private readonly IOptionsMonitor<DeploymentInformation> options;

        public DeploymentController(IOptionsMonitor<DeploymentInformation> options)
        {
            this.options = options;
        }

        [HttpGet("configuration")]
        public DeploymentInformation GetConfiguration()
        {
            return options.CurrentValue;
        }
    }
}