using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    public class EventsController : SignalBoxControllerBase
    {
        private readonly IEnvironmentProvider environmentProvider;
        private readonly ITenantProvider tenantProvider;
        private readonly CustomerEventsWorkflows workflows;
        private readonly ICustomerEventStore eventStore;

        public EventsController(IEnvironmentProvider environmentProvider,
                                ITenantProvider tenantProvider,
                                CustomerEventsWorkflows workflows,
                                ICustomerEventStore eventStore)
        {
            this.environmentProvider = environmentProvider;
            this.tenantProvider = tenantProvider;
            this.workflows = workflows;
            this.eventStore = eventStore;
        }

        /// <summary>Stores event data about one or more tracked users.</summary>
        [HttpPost]
        [AllowApiKey]
        [EnableCors(CorsPolicies.WebApiKeyPolicy)]
        public async Task<EventLoggingResponse> LogEvents([FromBody] List<EventDto> dto)
        {
            return await workflows.Ingest(dto.Select(d =>
            new CustomerEventsWorkflows.CustomerEventInput(tenantName: tenantProvider.RequestedTenantName,
                                                            d.GetCustomerId(),
                                                            d.EventId,
                                                            d.Timestamp,
                                                            environmentProvider.CurrentEnvironmentId,
                                                            d.RecommendationCorrelatorId,
                                                            d.SourceSystemId,
                                                            d.Kind,
                                                            d.EventType,
                                                            d.Properties)));
        }

        [HttpGet("{id}")]
        public async Task<CustomerEvent> GetEvent(string id)
        {
            var e = await eventStore.Read(id);
            return e;
        }
    }
}
