using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;

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
        [HttpGet("event-kind-names")]
        public IEnumerable<string> EventKindNames()
        {
            return workflows.GetEventKindNames();
        }

        /// <summary>Summarises the events that have been collected.</summary>
        [HttpGet("events")]
        public async Task<CustomerEventSummary> EventsSummary()
        {
            return await workflows.GenerateSummary();
        }

        /// <summary>Summarises the events that have been collected.</summary>
        [HttpGet("event-kind/{kind}")]
        public async Task<CustomerEventKindSummary> EventKindSummary(EventKinds kind)
        {
            var s = await workflows.GenerateSummaryForKind(kind);
            return new CustomerEventKindSummary(kind, s);
        }

        /// <summary>Summarises a timeline of event counts each month.</summary>
        [HttpGet("events/timeline/{kind}/{eventType}")]
        public async Task<EventCountTimeline> EventTimeline(EventKinds kind, string eventType)
        {
            return await workflows.GenerateTimeline(kind, HttpUtility.UrlDecode(eventType));
        }
    }
}
