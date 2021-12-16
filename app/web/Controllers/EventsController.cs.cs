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
        private readonly ILogger<EventsController> _logger;
        private readonly CustomerEventsWorkflows workflows;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICustomerEventStore eventStore;

        public EventsController(ILogger<EventsController> logger,
                                CustomerEventsWorkflows workflows,
                                IDateTimeProvider dateTimeProvider,
                                ICustomerEventStore eventStore)
        {
            _logger = logger;
            this.workflows = workflows;
            this.dateTimeProvider = dateTimeProvider;
            this.eventStore = eventStore;
        }

        /// <summary>Stores event data about one or more tracked users.</summary>
        [HttpPost]
        [AllowApiKey]
        [EnableCors(CorsPolicies.WebApiKeyPolicy)]
        public async Task<EventLoggingResponse> LogEvents([FromBody] List<EventDto> dto)
        {
            var enqueue = dto.Count > 100;
            return await workflows.AddEvents(dto.Select(d =>
            new CustomerEventsWorkflows.CustomerEventInput(d.GetCustomerId(),
                                                                 d.EventId,
                                                                 d.Timestamp,
                                                                 d.RecommendationCorrelatorId,
                                                                 d.SourceSystemId,
                                                                 d.Kind.ToEventKind(),
                                                                 d.EventType,
                                                                 d.Properties)), addToQueue: enqueue); // add to queue if available
        }

        [HttpGet("{id}")]
        public async Task<CustomerEvent> GetEvent(string id)
        {
            var e = await eventStore.Read(id);
            await eventStore.LoadMany(e, _ => _.Actions);
            return e;
        }
    }
}
