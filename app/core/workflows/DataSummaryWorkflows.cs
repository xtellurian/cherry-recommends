using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class DataSummaryWorkflows : IWorkflow
    {
        private readonly ITrackedUserEventStore eventStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ITelemetry telemetry;

        public DataSummaryWorkflows(ITrackedUserEventStore eventStore, IDateTimeProvider dateTimeProvider, ITelemetry telemetry)
        {
            this.eventStore = eventStore;
            this.dateTimeProvider = dateTimeProvider;
            this.telemetry = telemetry;
        }

        // private IEnumerable<MomentCount> EventsToMonthlyMoments(IEnumerable<TrackedUserEvent> events, string category = null)
        // {
        //     var stopwatch = new Stopwatch();
        //     stopwatch.Start();
        //     var moments = events
        //         .GroupBy(_ => _.Timestamp.TruncateToMonthStart())
        //         .Select(group => new MomentCount(category, group.Key, group.Count()));
        //     stopwatch.Stop();
        //     telemetry.TrackMetric("DataSummaryWorkflows_EventsToMonthlyMoments_ExecutionTimeSeconds", stopwatch.Elapsed.TotalSeconds);
        //     return moments;
        // }

        public async Task<TrackedUserEventSummary> GenerateSummary()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

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

            stopwatch.Stop();
            telemetry.TrackMetric("DataSummaryWorkflows_GenerateSummary_ExecutionTimeSeconds", stopwatch.Elapsed.TotalSeconds);
            return summary;
        }

        public async Task<EventCountTimeline> GenerateTimeline(string kind, string eventType)
        {
            var start = await eventStore.Min(_ => _.Timestamp);
            var end = await eventStore.Max(_ => _.Timestamp);

            // iterate through months and count events
            var moments = new List<MomentCount>();
            while (start.TruncateToMonthStart() < end.AddMonths(1).TruncateToMonthStart())
            {
                var count = await eventStore.Count(_ =>
                    _.Timestamp > start &&
                    _.Timestamp < start.AddMonths(1) &&
                    _.EventType == eventType &&
                    _.Kind == kind);
                moments.Add(new MomentCount(start, count));
                start = start.AddMonths(1);
            }

            return new EventCountTimeline(moments);
        }

        public async Task<Dashboard> GenerateDashboardData(string scope = null)
        {
            var dashboard = new Dashboard();
            var since = dateTimeProvider.Now.AddMonths(-3);
            var firstEvent = await eventStore.Min(_ => _.Timestamp);
            var lastEvent = await eventStore.Max(_ => _.Timestamp);
            var end = new DateTimeOffset(new DateTime(Math.Min(lastEvent.Ticks, dateTimeProvider.Now.Ticks)));


            if (scope == "eventType")
            {
                var types = await eventStore.ReadUniqueEventTypes();
                var moments = new List<MomentCount>();
                foreach (var t in types)
                {
                    var start = new DateTimeOffset(new DateTime(Math.Max(firstEvent.Ticks, since.Ticks)));
                    while (start.TruncateToMonthStart() < end.AddMonths(1).TruncateToMonthStart())
                    {
                        var eventCount = await eventStore.CountEventsOfType(t, start, start.AddMonths(1));
                        var moment = new MomentCount($"{t}", start, eventCount);
                        moments.Add(moment);
                        start = start.AddMonths(1);
                    }
                }
                dashboard = new Dashboard(moments);
            }
            else if (scope == "kind")
            {
                var kinds = await eventStore.ReadUniqueKinds();
                var moments = new List<MomentCount>();
                foreach (var k in kinds)
                {
                    var start = new DateTimeOffset(new DateTime(Math.Max(firstEvent.Ticks, since.Ticks)));
                    // iterate through months and count events
                    while (start.TruncateToMonthStart() < end.AddMonths(1).TruncateToMonthStart())
                    {
                        var eventCount = await eventStore.CountEventsOfKind(k, start, start.AddMonths(1));
                        var moment = new MomentCount($"{k}", start, eventCount);
                        moments.Add(moment);
                        start = start.AddMonths(1);
                    }
                }
                dashboard = new Dashboard(moments);
            }
            else
            {
                var kinds = await eventStore.ReadUniqueKinds();
                var moments = new List<MomentCount>();
                foreach (var k in kinds)
                {
                    var eventTypes = await eventStore.ReadUniqueEventTypes(k);

                    foreach (var t in eventTypes)
                    {
                        var start = new DateTimeOffset(new DateTime(Math.Max(firstEvent.Ticks, since.Ticks)));
                        while (start.TruncateToMonthStart() < end.AddMonths(1).TruncateToMonthStart())
                        {
                            var eventCount = await eventStore.CountEventsOfType(k, t, start, start.AddMonths(1));
                            var moment = new MomentCount($"{k}|{t}", start, eventCount);
                            moments.Add(moment);
                            start = start.AddMonths(1);
                        }
                    }
                }
                dashboard = new Dashboard(moments);
            }


            return dashboard;
        }
    }
}