using System;
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
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly TrackedUserEventsWorkflows workflows;
        private readonly ITrackedUserEventStore eventStore;

        public EventsController(ILogger<EventsController> logger,
                                TrackedUserEventsWorkflows workflows,
                                ITrackedUserEventStore eventStore)
        {
            _logger = logger;
            this.workflows = workflows;
            this.eventStore = eventStore;
        }

        [HttpPost]
        public async Task<object> LogEvents([FromBody] List<EventDto> dto)
        {
            await workflows.TrackUserEvents(dto.Select(d =>
            new TrackedUserEventsWorkflows.TrackedUserEventInput(d.CommonUserId,
                                                                 d.EventId,
                                                                 d.Timestamp,
                                                                 d.SourceSystemId,
                                                                 d.Kind,
                                                                 d.EventType,
                                                                 d.Properties)));

            return new object();
        }
    }
}
