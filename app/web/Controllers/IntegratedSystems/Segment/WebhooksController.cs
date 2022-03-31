using System.Collections.Generic;
using System.Linq;
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
    [SkipSegmentAnalytics]
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
        [HttpPost("receivers/{endpointId}", Name = "AcceptWebhook")]
        public async Task AcceptSegmentWebhook(string endpointId)
        {
            var body = await Request.GetRawBodyStringAsync();
            var headers = Request.Headers.Select(_ => _);
            if (Request.Headers.TryGetValue("x-signature", out var signature))
            {
                await workflows.ProcessWebhook(endpointId, body, headers, signature);
            }
            else
            {
                await workflows.ProcessWebhook(endpointId, body, headers, null);
            }
        }
    }
}
