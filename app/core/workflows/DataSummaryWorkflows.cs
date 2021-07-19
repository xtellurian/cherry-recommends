using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Workflows
{
    public class DataSummaryWorkflows : IWorkflow
    {
        private readonly ITrackedUserEventStore eventStore;
        private readonly ITrackedUserActionStore actionStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IProductRecommendationStore productRecommendationStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly ITelemetry telemetry;

        public DataSummaryWorkflows(ITrackedUserEventStore eventStore,
                                    ITrackedUserActionStore actionStore,
                                    IDateTimeProvider dateTimeProvider,
                                    IProductRecommendationStore productRecommendationStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    ITelemetry telemetry)
        {
            this.eventStore = eventStore;
            this.actionStore = actionStore;
            this.dateTimeProvider = dateTimeProvider;
            this.productRecommendationStore = productRecommendationStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.telemetry = telemetry;
        }

        public async Task<Paginated<TrackedUserAction>> LatestActions()
        {
            return await actionStore.Query(1);
        }

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
            var latestEvents = await eventStore.Latest(dateTimeProvider.Now.AddMonths(-1));
            var actions = await actionStore.Query(1);

            // get some recommendations
            var recommendations = new List<RecommendationEntity>();
            var parameterSetRecommendations = await parameterSetRecommendationStore.Query(1);
            recommendations.AddRange(parameterSetRecommendations.Items);
            var productRecommendations = await productRecommendationStore.Query(1);
            recommendations.AddRange(productRecommendations.Items);
            recommendations.Sort((x, y) => DateTimeOffset.Compare(y.LastUpdated, x.LastUpdated));

            return new Dashboard(latestEvents, actions.Items, recommendations);
        }
    }
}