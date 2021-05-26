using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryEventStore : ITrackedUserEventStore
    {
        private List<TrackedUserEvent> eventStore = new List<TrackedUserEvent>();

        public Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events)
        {
            events = events.Where(_ => _ != null); // remove nulls
            eventStore.AddRange(events);
            return Task.FromResult(events);
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string commonUserId)
        {
            return Task.FromResult(eventStore.Where(_ => _.CommonUserId == commonUserId));
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType)
        {
            return Task.FromResult(eventStore.Where(_ => _.EventType == eventType));
        }
    }
}