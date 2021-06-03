using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class DataSummaryController : SignalBoxControllerBase
    {
        private readonly ILogger<DataSummaryController> logger;
        private readonly DataSummaryWorkflows workflows;

        public DataSummaryController(ILogger<DataSummaryController> logger,
                                DataSummaryWorkflows workflows)
        {
            this.logger = logger;
            this.workflows = workflows;
        }

        [HttpGet("dashboard")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Dashboard> DashboardData(string scope = null)
        {
            return await workflows.GenerateDashboardData(scope);
        }

        /// <summary>Summarises the events that have been collected.</summary>
        [HttpGet("events")]
        public async Task<TrackedUserEventSummary> EventsSummary()
        {
            return await workflows.GenerateSummary();
        }

        /// <summary>Summarises a timeline of event counts each month.</summary>
        [HttpGet("events/timeline/{kind}/{eventType}")]
        public async Task<EventCountTimeline> EventTimeline(string kind, string eventType)
        {
            return await workflows.GenerateTimeline(kind, eventType);
        }
    }
}
