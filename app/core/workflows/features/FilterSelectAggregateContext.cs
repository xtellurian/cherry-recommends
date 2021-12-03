using System.Collections.Generic;
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
            bool moreEvents = true;
            int page = 1;
            var results = new List<TrackedUserEvent>();
            var isAllEvents = filter?.EventTypeMatch == null;
            while (moreEvents)
            {
                var e = await eventStore.ReadEventsForUser(page++, TrackedUser, _ => isAllEvents || (_.EventType == filter.EventTypeMatch));
                moreEvents = e.Pagination.HasNextPage;
                results.AddRange(e.Items);
            }

            this.Events = results;
        }
    }
}