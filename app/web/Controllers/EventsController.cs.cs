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
    public class EventsController : SignalBoxControllerBase
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

        /// <summary>Stores event data about one or more tracked users.</summary>
        [HttpPost]
        public async Task<EventLoggingResponse> LogEvents([FromBody] List<EventDto> dto)
        {
            var enqueue = dto.Count > 100;
            return await workflows.TrackUserEvents(dto.Select(d =>
            new TrackedUserEventsWorkflows.TrackedUserEventInput(d.CommonUserId,
                                                                 d.EventId,
                                                                 d.Timestamp,
                                                                 d.RecommendationCorrelatorId,
                                                                 d.SourceSystemId,
                                                                 d.Kind,
                                                                 d.EventType,
                                                                 d.Properties)), enqueue, !enqueue); // add to queue if available
        }

        /// <summary>Stores event data about one or more tracked users.</summary>
        [HttpGet]
        public async Task<EventsResponse> EventsForTrackedUser(string commonUserId)
        {
            if (commonUserId == null)
            {
                throw new BadRequestException("commonUserId cannot be null");
            }
            else
            {
                return new EventsResponse(await eventStore.ReadEventsForUser(commonUserId));
            }
        }
    }
}
