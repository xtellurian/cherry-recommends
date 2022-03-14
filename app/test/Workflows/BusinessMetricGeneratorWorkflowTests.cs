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
    public class BusinessMetricGeneratorWorkflowTests
    {
        public static Customer Customer1 = new Customer("customer1", "customer1") { Id = 1 };
        public static Customer Customer2 = new Customer("customer2", "customer2") { Id = 2 };
        public static Business Business1 = new Business("business1", "business1");

        public static async IAsyncEnumerable<Business> GetBusinesses()
        {
            yield return Business1;
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
                                    }),
                new CustomerEvent(Customer2,
                                    "event3",
                                    DateTime.Now,
                                    null,
                                    EventKinds.Custom,
                                    "custom",
                                    new Dictionary<string, object>()
                                    {
                                        { "purchase", 20 }
                                    })
            };
            return eventList;
        }

        [Theory]
        [InlineData(AggregationTypes.Sum, null, MetricValueType.Numeric, 3)]
        [InlineData(AggregationTypes.Mean, null, MetricValueType.Numeric, 1)]
        [InlineData(AggregationTypes.Sum, "purchase", MetricValueType.Numeric, 36)]
        [InlineData(AggregationTypes.Mean, "purchase", MetricValueType.Numeric, 12)]
        [InlineData(AggregationTypes.Sum, null, MetricValueType.Categorical, 3)]
        public async Task RunBusinessMetricGeneration(AggregationTypes aggregationType, string propertyMatch, MetricValueType metricValueType, double expectedValue)
        {
            var mockLogger = Utility.MockLogger<BusinessMetricGeneratorWorkflow>();
            var mockMetricStore = new Mock<IMetricStore>();
            var mockCustomerEventStore = new Mock<ICustomerEventStore>();
            var mockMetricGeneratorStore = new Mock<IMetricGeneratorStore>();
            var mockContext = Utility.MockStorageContext();
            var mockBusinessMetricValueStore = new Mock<IBusinessMetricValueStore>();
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockWebhookSenderClient = new Mock<IWebhookSenderClient>();
            var mockTelemetry = new Mock<ITelemetry>();
            var mockAggregateLogger = Utility.MockLogger<FilterSelectAggregateWorkflow>();
            var aggregateWorkflow = new FilterSelectAggregateWorkflow(mockAggregateLogger.Object);

            BusinessMetricGeneratorWorkflow workflow = new BusinessMetricGeneratorWorkflow(
                mockBusinessMetricValueStore.Object,
                mockBusinessStore.Object,
                mockCustomerEventStore.Object,
                mockMetricStore.Object,
                mockMetricGeneratorStore.Object,
                aggregateWorkflow,
                mockWebhookSenderClient.Object,
                mockTelemetry.Object,
                mockLogger.Object
            );

            List<FilterSelectAggregateStep> steps = new List<FilterSelectAggregateStep>
            {
                new FilterSelectAggregateStep(1, new SelectStep(propertyMatch)),
                new FilterSelectAggregateStep(2, new AggregateStep(){ AggregationType = aggregationType } )
            };

            var metric = new Metric("metric1", "metric1", metricValueType, MetricScopes.Business);
            var metricGenerator = MetricGenerator.CreateFilterSelectAggregateGenerator(metric, steps, MetricGeneratorTimeWindow.AllTime);

            mockBusinessStore.Setup(_ => _.Iterate(It.IsAny<Expression<Func<Business, bool>>>(), It.IsAny<IterateOrderBy>()))
                .Returns(GetBusinesses);

            mockCustomerEventStore.Setup(_ => _.ReadEventsForBusiness(It.Is<Business>(_ => _.CommonId == Business1.CommonId), It.IsAny<EventQueryOptions>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(GetCustomerEvents);

            var businessMetric = new BusinessMetricValue(Business1, metric, expectedValue, 1);
            mockBusinessMetricValueStore
                .Setup(_ => _.Create(It.Is<BusinessMetricValue>(_ => _.Business == Business1)))
                .ReturnsAsync(businessMetric);

            mockBusinessMetricValueStore.Setup(_ => _.Context).Returns(mockContext.Object);
            mockMetricStore.Setup(_ => _.Context).Returns(mockContext.Object);

            var result = await workflow.RunBusinessMetricGeneration(metricGenerator);

            mockContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));
            mockBusinessMetricValueStore.Verify(_ => _.Create(It.Is<BusinessMetricValue>(m => m.NumericValue == expectedValue)));
            Assert.Equal(1, result.TotalWrites);
        }
    }
}