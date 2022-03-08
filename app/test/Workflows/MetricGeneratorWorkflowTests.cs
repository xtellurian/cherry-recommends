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

        static RecommenderTriggersWorkflows CreateRecommenderTriggersWorkFlows()
        {
            var mockHistoricStore = new Mock<IHistoricCustomerMetricStore>();
            var mockItemsRecommenderStore = new Mock<IItemsRecommenderStore>();
            var mockParameterSetRecommenderStore = new Mock<IParameterSetRecommenderStore>();
            var mockTriggerLogger = Utility.MockLogger<RecommenderTriggersWorkflows>();
            var mockContext = Utility.MockStorageContext();
            var dateTimeProvider = new SystemDateTimeProvider();

            ItemsRecommender itemsRecommender = new ItemsRecommender("recommender1", "recommender1", null, null, null, new RecommenderSettings(), null);
            var invokationLogEntry = new InvokationLogEntry(itemsRecommender, DateTimeOffset.Now);
            var recommendingContext = new RecommendingContext(invokationLogEntry, null);

            List<ScoredRecommendableItem> scoredItems = new List<ScoredRecommendableItem>()
            {
                new ScoredRecommendableItem(new RecommendableItem("item1", "item1", 1, 1, BenefitType.Fixed, 1, PromotionType.Discount, null), 1)
            };
            ItemsRecommendation itemsRecommendation = new ItemsRecommendation(itemsRecommender, recommendingContext, scoredItems);

            var mockItemsLogger = Utility.MockLogger<ItemsRecommenderInvokationWorkflows>();
            var mockRecommendationCache = new Mock<IRecommendationCache<ItemsRecommender, ItemsRecommendation>>();
            var mockModelClientFactory = new Mock<IRecommenderModelClientFactory>();
            var mockCustomerWorkflow = new Mock<ICustomerWorkflow>();
            var mockBusinessWorkflow = new Mock<IBusinessWorkflow>();
            var mockItemStore = new Mock<IRecommendableItemStore>();
            var mockWebhookSenderClient = new Mock<IWebhookSenderClient>();
            var mockCorrelatorStore = new Mock<IRecommendationCorrelatorStore>();
            var mockItemsRecommendationStore = new Mock<IItemsRecommendationStore>();

            var itemsRecommenderInvokationWorkflows = new ItemsRecommenderInvokationWorkflows(
                mockItemsLogger.Object,
                mockContext.Object,
                dateTimeProvider,
                mockRecommendationCache.Object,
                mockModelClientFactory.Object,
                mockCustomerWorkflow.Object,
                mockBusinessWorkflow.Object,
                mockHistoricStore.Object,
                mockItemStore.Object,
                mockWebhookSenderClient.Object,
                mockCorrelatorStore.Object,
                mockItemsRecommenderStore.Object,
                mockItemsRecommendationStore.Object);

            var mockParameterSetLogger = Utility.MockLogger<ParameterSetRecommenderInvokationWorkflows>();
            var mockParameterRecommendationCache = new Mock<IRecommendationCache<ParameterSetRecommender, ParameterSetRecommendation>>();
            var mockParameterSetRecommendationStore = new Mock<IParameterSetRecommendationStore>();

            var parameterSetRecommenderInvokationWorkflows = new ParameterSetRecommenderInvokationWorkflows(
                mockParameterSetLogger.Object,
                mockContext.Object,
                dateTimeProvider,
                mockParameterRecommendationCache.Object,
                mockCorrelatorStore.Object,
                mockParameterSetRecommenderStore.Object,
                mockParameterSetRecommendationStore.Object,
                mockWebhookSenderClient.Object,
                mockHistoricStore.Object,
                mockCustomerWorkflow.Object,
                mockModelClientFactory.Object);

            List<ItemsRecommender> recommenderList = new List<ItemsRecommender>() { itemsRecommender };
            Paginated<ItemsRecommender> paginatedItems = new Paginated<ItemsRecommender>(recommenderList, 1, 1, 1);
            mockItemsRecommenderStore.Setup(_ => _.Query(It.IsAny<int>(), It.IsAny<Expression<Func<ItemsRecommender, bool>>>())).ReturnsAsync(paginatedItems);

            Paginated<ParameterSetRecommender> paginatedParameters = new Paginated<ParameterSetRecommender>(new List<ParameterSetRecommender>(), 1, 0, 1);
            mockParameterSetRecommenderStore.Setup(_ => _.Query(It.IsAny<int>(), It.IsAny<Expression<Func<ParameterSetRecommender, bool>>>())).ReturnsAsync(paginatedParameters);

            return new RecommenderTriggersWorkflows(
                mockTriggerLogger.Object,
                Utility.MockTelemetry().Object,
                mockItemsRecommenderStore.Object,
                mockParameterSetRecommenderStore.Object,
                itemsRecommenderInvokationWorkflows,
                parameterSetRecommenderInvokationWorkflows,
                mockHistoricStore.Object);
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
            var mockRecommenderTriggerWorkFlow = new Mock<RecommenderTriggersWorkflows>();
            var mockHubspotWorkflow = new Mock<HubspotPushWorkflows>();
            var mockWebhookClient = new Mock<IWebhookSenderClient>();
            var mockTelemetry = new Mock<ITelemetry>();
            var mockContext = Utility.MockStorageContext();

            RecommenderTriggersWorkflows recommenderTriggerWorkflow = CreateRecommenderTriggersWorkFlows();

            MetricGeneratorWorkflows workflow = new MetricGeneratorWorkflows(
                mockCustomerStore.Object,
                mockMetricStore.Object,
                mockCustomerEventStore.Object,
                mockMetricGeneratorStore.Object,
                mockHistoricStore.Object,
                recommenderTriggerWorkflow,
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