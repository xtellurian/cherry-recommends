using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class WebhooksController : SignalBoxControllerBase
    {
        private readonly ILogger<WebhooksController> logger;
        private readonly WebhookWorkflows workflows;

        public WebhooksController(ILogger<WebhooksController> logger, WebhookWorkflows workflows)
        {
            this.logger = logger;
            this.workflows = workflows;
        }

        /// <summary>An endpoint for a created webhook.</summary>
        [HttpPost("receivers/{endpointId}")]
        public async Task AcceptSegmentWebhook(string endpointId)
        {
            var body = await Request.GetRawBodyStringAsync();
            if (Request.Headers.TryGetValue("x-signature", out var signature))
            {
                await workflows.ProcessWebhook(endpointId, body, signature);
            }
            else
            {
                await workflows.ProcessWebhook(endpointId, body, null);
            }
        }
    }
}
