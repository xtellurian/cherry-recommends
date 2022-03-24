using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Metrics;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class MetricGeneratorTests
    {
        public static Customer Customer1 = new Customer("customer1", "customer1");

        public static async IAsyncEnumerable<Customer> GetCustomers()
        {
            yield return Customer1;
            await Task.CompletedTask;
        }

        public static IEnumerable<CustomerEvent> GetCustomerEvents()
        {
            List<CustomerEvent> eventList = new List<CustomerEvent>()
            {
                new CustomerEvent(Customer1,
                                    "event1",
                                    DateTime.Now,
                                    null,
                                    EventKinds.Custom,
                                    "custom",
                                    new Dictionary<string, object>()
                                    {
                                        { "purchase", 10 }
                                    }),

                new CustomerEvent(Customer1,
                                    "event2",
                                    DateTime.Now,
                                    null,
                                    EventKinds.Custom,
                                    "custom",
                                    new Dictionary<string, object>()
                                    {
                                        { "purchase", 6 }
                                    })
            };
            return eventList;
        }

        [Theory]
        [InlineData(AggregationTypes.Sum, null, 2)]
        [InlineData(AggregationTypes.Mean, null, 1)]
        [InlineData(AggregationTypes.Sum, "purchase", 16)]
        [InlineData(AggregationTypes.Mean, "purchase", 8)]
        public async Task RunMetricGeneration_FilterSelectAggregate(AggregationTypes aggregationType, string propertyMatch, double expectedValue)
        {
            var dateTimeProvider = new SystemDateTimeProvider();
            var mockLogger = Utility.MockLogger<MetricGeneratorWorkflows>();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var mockMetricStore = new Mock<IMetricStore>();
            var mockCustomerEventStore = new Mock<ICustomerEventStore>();
            var mockMetricGeneratorStore = new Mock<IMetricGeneratorStore>();
            var mockHistoricStore = new Mock<IHistoricCustomerMetricStore>();
            var mockRecommenderTriggerWorkflow = new Mock<IRecommenderMetricTriggersWorkflow>();
            var mockHubspotWorkflow = new Mock<HubspotPushWorkflows>();
            var mockWebhookClient = new Mock<IWebhookSenderClient>();
            var mockTelemetry = new Mock<ITelemetry>();
            var mockContext = Utility.MockStorageContext();
            var mockAggregateLogger = Utility.MockLogger<FilterSelectAggregateWorkflow>();
            var aggregateWorkflow = new FilterSelectAggregateWorkflow(mockAggregateLogger.Object);

            MetricGeneratorWorkflows workflow = new MetricGeneratorWorkflows(
                mockCustomerStore.Object,
                mockMetricStore.Object,
                mockCustomerEventStore.Object,
                mockMetricGeneratorStore.Object,
                mockHistoricStore.Object,
                mockRecommenderTriggerWorkflow.Object,
                aggregateWorkflow,
                null,
                mockWebhookClient.Object,
                dateTimeProvider,
                mockTelemetry.Object,
                mockLogger.Object
            );

            List<FilterSelectAggregateStep> steps = new List<FilterSelectAggregateStep>
            {
                new FilterSelectAggregateStep(1, new SelectStep(propertyMatch)),
                new FilterSelectAggregateStep(2, new AggregateStep(){ AggregationType = aggregationType } )
            };

            var metric = new Metric("metric1", "metric1", MetricValueType.Numeric, MetricScopes.Customer);
            var metricGenerator = MetricGenerator.CreateFilterSelectAggregateGenerator(metric, steps, MetricGeneratorTimeWindow.AllTime);

            mockCustomerStore.Setup(_ => _.Iterate(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<IterateOrderBy>()))
                .Returns(GetCustomers);

            mockCustomerEventStore.Setup(_ => _.ReadEventsForUser(It.Is<Customer>(_ => _.CommonId == Customer1.CommonId), It.IsAny<EventQueryOptions>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(GetCustomerEvents);

            mockMetricStore
                .Setup(_ => _.ExistsFromCommonId(It.Is<string>(_ => _ == metric.CommonId)))
                .ReturnsAsync(true);
            mockMetricStore
                .Setup(_ => _.ReadFromCommonId(It.Is<string>(_ => _ == metric.CommonId)))
                .ReturnsAsync(metric);

            var historicMetric = new HistoricCustomerMetric(Customer1, metric, expectedValue, 1);
            mockHistoricStore
                .Setup(_ => _.Create(It.Is<HistoricCustomerMetric>(_ => _.Customer == Customer1)))
                .ReturnsAsync(historicMetric);

            mockHistoricStore.Setup(_ => _.Context).Returns(mockContext.Object);
            mockMetricStore.Setup(_ => _.Context).Returns(mockContext.Object);

            var result = await workflow.RunMetricGeneration(metricGenerator);

            mockContext.Verify(_ => _.SaveChanges(), Times.Exactly(2));  // metric store and historicCustomerMetricStore
            mockHistoricStore.Verify(_ => _.Create(It.Is<HistoricCustomerMetric>(m => m.NumericValue == expectedValue)));
            Assert.Equal(1, result.TotalWrites);
        }
    }
}
