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
        private readonly IItemsRecommendationStore promoRecommendationStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly ITelemetry telemetry;

        public DataSummaryWorkflows(ICustomerEventStore eventStore,
                                    ILogger<DataSummaryWorkflows> logger,
                                    ICustomerStore customerStore,
                                    IDateTimeProvider dateTimeProvider,
                                    IItemsRecommendationStore promoRecommendationStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore,
                                    ITelemetry telemetry)
        {
            this.eventStore = eventStore;
            this.logger = logger;
            this.customerStore = customerStore;
            this.dateTimeProvider = dateTimeProvider;
            this.promoRecommendationStore = promoRecommendationStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.telemetry = telemetry;
        }

        public IEnumerable<string> GetEventKindNames()
        {
            return Enum.GetNames(typeof(EventKinds));
        }

        public async Task<CustomerEventSummary> GenerateSummary()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var kinds = new List<EventKinds>
            {
                EventKinds.Custom,
                EventKinds.PropertyUpdate,
                EventKinds.Behaviour,
                EventKinds.PageView,
                EventKinds.Identify,
                EventKinds.ConsumeRecommendation
            };

            var summary = new CustomerEventSummary();
            foreach (var k in kinds)
            {
                var kindSummary = await GenerateSummaryForKind(k);
                summary.Add(k, kindSummary);
            }

            stopwatch.Stop();
            telemetry.TrackMetric("DataSummaryWorkflows_GenerateSummary_ExecutionTimeSeconds", stopwatch.Elapsed.TotalSeconds);
            return summary;
        }

        public async Task<EventKindSummary> GenerateSummaryForKind(EventKinds k)
        {
            var totalOfKind = await eventStore.Count(_ => _.EventKind == k);
            var eventTypes = await eventStore.ReadUniqueEventTypes(k);
            var counts = new Dictionary<string, EventStats>();
            foreach (var t in eventTypes)
            {
                var instances = await eventStore.Count(_ => _.EventKind == k && _.EventType == t);
                var users = await eventStore.CountTrackedUsers(_ => _.EventKind == k && _.EventType == t);
                double fractionOfKind = (double)instances / totalOfKind;
                var stats = new EventStats(instances, fractionOfKind, users);
                counts.Add(t, stats);
            }

            return new EventKindSummary(counts);
        }

        public async Task<EventCountTimeline> GenerateTimeline(EventKinds kind, string eventType)
        {
            var start = await eventStore.Min(_ => _.Timestamp);
            var end = await eventStore.Max(_ => _.Timestamp);

            // iterate through months and count events
            var moments = new List<MomentCount>();
            while (start.TruncateToMonthStart() < end.AddMonths(1).TruncateToMonthStart())
            {
                var count = await eventStore.Count(_ =>
                    _.Timestamp >= start &&
                    _.Timestamp < start.AddMonths(1) &&
                    _.EventType == eventType &&
                    _.EventKind == kind);
                moments.Add(new MomentCount(start, count));
                start = start.AddMonths(1);
            }

            return new EventCountTimeline(moments);
        }

        public async Task<GeneralSummary> CalculateGeneralSummary()
        {
            var now = dateTimeProvider.Now;
            var yesterday = now.AddDays(-1);
            var totalCustomers = await customerStore.Count();
            int eventCount24Hour = 0;

            var timeCutoff = dateTimeProvider.Now.AddMonths(-1);
            try
            {
                eventCount24Hour = await eventStore.Count(_ => _.Created >= yesterday);
            }
            catch (Exception ex)
            {
                logger.LogError("Timeout querying 24H event count", ex);
                throw;
            }

            int recommendationCount24Hour = 0;
            try
            {
                recommendationCount24Hour += await parameterSetRecommendationStore.Count(_ => _.Created >= yesterday);
                recommendationCount24Hour += await promoRecommendationStore.Count(_ => _.Created >= yesterday);
            }
            catch (Exception ex)
            {
                logger.LogError("Timeout querying recommendations", ex);
                throw;
            }

            return new GeneralSummary(totalCustomers, eventCount24Hour, recommendationCount24Hour);
        }
    }
}