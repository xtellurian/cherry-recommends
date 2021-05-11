using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryEventStore : ITrackedUserEventStore
    {
        private List<TrackedUserEvent> eventStore = new List<TrackedUserEvent>();

        public Task AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events)
        {
            events = events.Where(_ => _ != null); // remove nulls
            eventStore.AddRange(events);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string trackedUserExternalId)
        {
            return Task.FromResult(eventStore.Where(_ => _.TrackedUserExternalId == trackedUserExternalId));
        }

#nullable enable
        public Task<IEnumerable<TrackedUserEvent>> ReadEventsForKey(string key, string? value = null)
        {
            IEnumerable<TrackedUserEvent> values;
            if (key == null)
            {
                values = eventStore.ToList();
            }
            else if (value == null)
            {
                // no value required, just any with this key
                values = eventStore
                   .Where(p => p.Key == key)
                   .ToList();
            }
            else
            {
                values = eventStore
                   .Where(p => p.Key == key && string.Equals(p.LogicalValue, value))
                   .ToList();
            }

            return Task.FromResult(values);
        }


    }
}