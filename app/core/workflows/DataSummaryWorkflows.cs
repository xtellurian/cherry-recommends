using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class DataSummaryWorkflows : IWorkflow
    {
        private readonly ITrackedUserEventStore eventStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public DataSummaryWorkflows(ITrackedUserEventStore eventStore, IDateTimeProvider dateTimeProvider)
        {
            this.eventStore = eventStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        private IEnumerable<MomentCount> EventsToMonthlyMoments(IEnumerable<TrackedUserEvent> events, string category = null)
        {
            return events
                .GroupBy(_ => _.Timestamp.TruncateToMonthStart())
                .Select(group => new MomentCount(category, group.Key, group.Count()));
        }

        public async Task<TrackedUserEventSummary> GenerateSummary()
        {
            var kinds = await eventStore.ReadUniqueKinds();

            var summary = new TrackedUserEventSummary();
            foreach (var k in kinds)
            {
                var totalOfKind = await eventStore.Count(_ => _.Kind == k);
                var eventTypes = await eventStore.ReadUniqueEventTypes(k);
                var counts = new Dictionary<string, EventStats>();
                foreach (var t in eventTypes)
                {
                    var instances = await eventStore.Count(_ => _.Kind == k && _.EventType == t);
                    var users = await eventStore.CountTrackedUsers(_ => _.Kind == k && _.EventType == t);
                    double fractionOfKind = (double)instances / totalOfKind;
                    var stats = new EventStats(instances, fractionOfKind, users);
                    counts.Add(t, stats);
                }
                summary.AddKind(k, counts);
            }
            return summary;
        }

        public async Task<EventCountTimeline> GenerateTimeline(string kind, string eventType)
        {
            var events = await eventStore.ReadEventsOfType(kind, eventType);
            var moments = EventsToMonthlyMoments(events).OrderByDescending(_ => _.Timestamp);
            return new EventCountTimeline(moments);
        }

        public async Task<Dashboard> GenerateDashboardData(string scope = null)
        {
            var dashboard = new Dashboard();
            var since = dateTimeProvider.Now.AddMonths(-3);
            var until = dateTimeProvider.Now;

            if (scope == "eventType")
            {
                var types = await eventStore.ReadUniqueEventTypes();
                var allMoments = new List<MomentCount>();
                foreach (var t in types)
                {
                    var events = await eventStore.ReadEventsOfType(t, since, until);
                    var moments = EventsToMonthlyMoments(events, t);
                    allMoments.AddRange(moments);
                }
                dashboard = new Dashboard(allMoments);
            }
            else if (scope == "kind")
            {
                var kinds = await eventStore.ReadUniqueKinds();
                var allMoments = new List<MomentCount>();
                foreach (var k in kinds)
                {
                    var events = await eventStore.ReadEventsOfKind(k, since, until);
                    var moments = EventsToMonthlyMoments(events, k);
                    allMoments.AddRange(moments);
                }
                dashboard = new Dashboard(allMoments);
            }
            else
            {
                var kinds = await eventStore.ReadUniqueKinds();
                var allMoments = new List<MomentCount>();
                foreach (var k in kinds)
                {
                    var eventTypes = await eventStore.ReadUniqueEventTypes(k);

                    foreach (var t in eventTypes)
                    {
                        var events = await eventStore.ReadEventsOfType(k, t, since, until);
                        var moments = EventsToMonthlyMoments(events, $"{k}|{t}");
                        allMoments.AddRange(moments);
                    }
                }
                dashboard = new Dashboard(allMoments);
            }


            return dashboard;
        }
    }
}