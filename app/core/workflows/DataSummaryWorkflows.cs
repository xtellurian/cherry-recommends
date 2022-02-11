using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Workflows
{
    public class DataSummaryWorkflows : IWorkflow
    {
        private readonly ICustomerEventStore eventStore;
        private readonly ILogger<DataSummaryWorkflows> logger;
        private readonly ICustomerStore customerStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IItemsRecommendationStore itemsRecommendationStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly ITelemetry telemetry;

        public DataSummaryWorkflows(ICustomerEventStore eventStore,
                                    ILogger<DataSummaryWorkflows> logger,
                                    ICustomerStore customerStore,
                                    IDateTimeProvider dateTimeProvider,
                                    IItemsRecommendationStore itemsRecommendationStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    ITelemetry telemetry)
        {
            this.eventStore = eventStore;
            this.logger = logger;
            this.customerStore = customerStore;
            this.dateTimeProvider = dateTimeProvider;
            this.itemsRecommendationStore = itemsRecommendationStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.telemetry = telemetry;
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
            IEnumerable<CustomerEvent> latestEvents = new List<CustomerEvent>();
            var totalCustomers = await customerStore.Count();
            var recommendations = new List<RecommendationEntity>();
            var timeCutoff = dateTimeProvider.Now.AddMonths(-1);
            try
            {
                latestEvents = await eventStore.Latest(timeCutoff);
            }
            catch (Exception ex)
            {
                logger.LogError("Timeout querying latest events", ex);
                throw;
            }
            // try
            // {
            //     // var actionsResponse = await actionStore.Query(1, _ => _.Timestamp > timeCutoff);
            //     // actions = actionsResponse.Items;
            // }
            // catch (Exception ex)
            // {
            //     logger.LogError("Timeout querying latest actions", ex);
            //     throw;
            // }

            try
            {
                var parameterSetRecommendations = await parameterSetRecommendationStore.Query(1);
                recommendations.AddRange(parameterSetRecommendations.Items);
                var itemsRecommendations = await itemsRecommendationStore.Query(1);
                recommendations.AddRange(itemsRecommendations.Items);
                recommendations.Sort((x, y) => DateTimeOffset.Compare(y.LastUpdated, x.LastUpdated));
                recommendations = recommendations.Take(20).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError("Timeout querying recommendations", ex);
                throw;
            }



            return new Dashboard(totalCustomers, latestEvents, recommendations);
        }
    }
}