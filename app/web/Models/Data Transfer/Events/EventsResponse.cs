using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class EventsResponse
    {
        public EventsResponse(IEnumerable<TrackedUserEvent> events)
        {
            Events = events;
        }

        public IEnumerable<string> Kinds => Events.Select(_ => _.Kind).Distinct();
        public IEnumerable<string> EventTypes => Events.Select(_ => _.EventType).Distinct();
        public IEnumerable<TrackedUserEvent> Events { get; set; }
    }
}