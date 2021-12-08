using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Features.Generators
{
    public class FilterSelectAggregateContext
    {
        private readonly ITrackedUserEventStore eventStore;

        public FilterSelectAggregateContext(TrackedUser trackedUser, Feature feature, ITrackedUserEventStore eventStore)
        {
            this.TrackedUser = trackedUser;
            Feature = feature;
            this.eventStore = eventStore;
        }

        public List<FilterSelectAggregateStep> Steps { get; set; }
        public TrackedUser TrackedUser { get; set; }
        public Feature Feature { get; }
        public List<TrackedUserEvent> Events { get; private set; }

        public async Task LoadEventsIntoContext(FilterStep filter = null)
        {
            var isAllEvents = filter?.EventTypeMatch == null;
            var result = await eventStore.ReadEventsForUser(TrackedUser, new EventQueryOptions
            {
                Filter = _ => isAllEvents || (_.EventType == filter.EventTypeMatch)
            });

            this.Events = result.ToList();
        }
    }
}