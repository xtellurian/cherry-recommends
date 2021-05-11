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
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly SegmentWorkflows segmentWorkflows;
        private readonly ITrackedUserStore userStore;
        private readonly IRuleStore ruleStore;
        private readonly ITrackedUserEventStore eventStore;

        public EventsController(ILogger<EventsController> logger,
                                IDateTimeProvider dateTimeProvider,
                                SegmentWorkflows segmentWorkflows,
                                ITrackedUserStore userStore,
                                IRuleStore ruleStore,
                                ITrackedUserEventStore eventStore)
        {
            _logger = logger;
            this.dateTimeProvider = dateTimeProvider;
            this.segmentWorkflows = segmentWorkflows;
            this.userStore = userStore;
            this.ruleStore = ruleStore;
            this.eventStore = eventStore;
        }

        [HttpPost]
        public async Task<object> LogEvent([FromBody] List<EventDto> dto)
        {
            var events = new List<TrackedUserEvent>();
            var newUsers = await userStore.CreateIfNotExists(dto.Select(_ => _.TrackedUserExternalId));

            foreach (var d in dto)
            {
                events.Add(new TrackedUserEvent(d.TrackedUserExternalId, dateTimeProvider.Now, d.Key, d.LogicalValue, d.NumericValue));
            }
            await eventStore.AddTrackedUserEvents(events);
            await ProcessRules(events);
            return new object();
        }

        private async Task ProcessRules(List<TrackedUserEvent> events)
        {
            var rules = await ruleStore.List();
            foreach (var r in rules)
            {
                await segmentWorkflows.ProcessRule(r, events);
            }
        }
    }
}
