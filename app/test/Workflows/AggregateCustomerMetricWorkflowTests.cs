using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Metrics;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Files;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class AggregateCustomerMetricWorkflowTests
    {

        [Theory]
        [InlineData(AggregationTypes.Mean, 16.1, 5, 16.1)]
        [InlineData(AggregationTypes.Sum, 10.5, 4, 42)]
        public async Task CanCalculateMeanOfNumericCustomerMetric(AggregationTypes aggType, double weeklyMean, int distinct, double expectedValue)
        {
            var now = DateTime.Now;
            var mockStorageContext = new Mock<IStorageContext>();
            var mockHistoricCustomerMetricStore = new Mock<IHistoricCustomerMetricStore>();
            var mockGlobalMetricValueStore = new Mock<IGlobalMetricValueStore>();

            var metric = new Metric
            {
                Id = 1,
                ValueType = MetricValueType.Numeric,
                Scope = MetricScopes.Global
            };
            var definition = new AggregateCustomerMetric
            {
                AggregationType = aggType,
                MetricId = metric.Id
            };

            // setup mocks
            var historicValues = new List<CustomerMetricWeeklyNumericAggregate>
            {
                new CustomerMetricWeeklyNumericAggregate
                {
                    FirstOfWeek = now.AddDays(-7).FirstDayOfWeek(DayOfWeek.Monday),
                    LastOfWeek = now.AddDays(-7).FirstDayOfWeek(DayOfWeek.Monday).AddDays(6),
                    MetricId = metric.Id,
                    WeeklyDistinctCustomerCount = distinct,
                    WeeklyMeanNumericValue = weeklyMean
                },
                new CustomerMetricWeeklyNumericAggregate
                {
                    FirstOfWeek = now.AddDays(-14).FirstDayOfWeek(DayOfWeek.Monday),
                    LastOfWeek = now.AddDays(-14).FirstDayOfWeek(DayOfWeek.Monday).AddDays(6),
                    MetricId = metric.Id,
                    WeeklyDistinctCustomerCount = 5,
                    WeeklyMeanNumericValue = 12.1
                }
            };

            mockHistoricCustomerMetricStore.Setup(
                x => x.GetAggregateMetricValuesNumeric(It.Is<Metric>(_ => _.Id == metric.Id), It.Is<int>(_ => _ == 0)))
                .ReturnsAsync(historicValues);
            mockGlobalMetricValueStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var sut = new AggregateCustomerMetricWorkflow(mockHistoricCustomerMetricStore.Object, mockGlobalMetricValueStore.Object);

            var generator = MetricGenerator.ForAggregateCustomerMetric(metric, definition);
            await sut.RunAggregateCustomerMetricWorkflow(generator);


            mockHistoricCustomerMetricStore.Verify(
                _ => _.GetAggregateMetricValuesNumeric(It.Is<Metric>(_ => _.Id == metric.Id), It.Is<int>(_ => _ == 0)), Times.Exactly(1));

            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));

            mockGlobalMetricValueStore.Verify(_ => _.Create(It.Is<GlobalMetricValue>(_ => _.NumericValue == expectedValue)));
        }

        [Fact]
        public async Task CanCalculateSumOfStringCustomerMetric()
        {
            var now = DateTime.Now;
            var mockStorageContext = new Mock<IStorageContext>();
            var mockHistoricCustomerMetricStore = new Mock<IHistoricCustomerMetricStore>();
            var mockGlobalMetricValueStore = new Mock<IGlobalMetricValueStore>();

            var metric = new Metric
            {
                Id = 2,
                ValueType = MetricValueType.Categorical,
                Scope = MetricScopes.Global
            };
            var definition = new AggregateCustomerMetric
            {
                AggregationType = AggregationTypes.Sum,
                MetricId = metric.Id,
                CategoricalValue = "dogs"
            };

            // setup mocks
            var latestValueToExpect = 7;
            var historicValues = new List<CustomerMetricWeeklyStringAggregate>
            {
                new CustomerMetricWeeklyStringAggregate
                {
                    FirstOfWeek = now.AddDays(-7).FirstDayOfWeek(DayOfWeek.Monday),
                    LastOfWeek = now.AddDays(-7).FirstDayOfWeek(DayOfWeek.Monday).AddDays(6),
                    MetricId = metric.Id,
                    StringValue = "dogs",
                    WeeklyDistinctCustomerCount = latestValueToExpect,
                    WeeklyValueCount = 7
                },
                new CustomerMetricWeeklyStringAggregate
                {
                    FirstOfWeek = now.AddDays(-14).FirstDayOfWeek(DayOfWeek.Monday),
                    LastOfWeek = now.AddDays(-14).FirstDayOfWeek(DayOfWeek.Monday).AddDays(6),
                    MetricId = metric.Id,
                    StringValue = "dogs",
                    WeeklyDistinctCustomerCount = 5,
                    WeeklyValueCount = 5
                },
                new CustomerMetricWeeklyStringAggregate
                {
                    FirstOfWeek = now.AddDays(-7).FirstDayOfWeek(DayOfWeek.Monday),
                    LastOfWeek = now.AddDays(-7).FirstDayOfWeek(DayOfWeek.Monday).AddDays(6),
                    MetricId = metric.Id,
                    StringValue = "cats",
                    WeeklyDistinctCustomerCount = 3,
                    WeeklyValueCount = 3
                },
                new CustomerMetricWeeklyStringAggregate
                {
                    FirstOfWeek = now.AddDays(-14).FirstDayOfWeek(DayOfWeek.Monday),
                    LastOfWeek = now.AddDays(-14).FirstDayOfWeek(DayOfWeek.Monday).AddDays(6),
                    MetricId = metric.Id,
                    StringValue = "cats",
                    WeeklyDistinctCustomerCount = 7,
                    WeeklyValueCount = 7
                }
            };

            mockHistoricCustomerMetricStore.Setup(
                x => x.GetAggregateMetricValuesString(It.Is<Metric>(_ => _.Id == metric.Id), It.Is<int>(_ => _ == 0)))
                .ReturnsAsync(historicValues);
            mockGlobalMetricValueStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var sut = new AggregateCustomerMetricWorkflow(mockHistoricCustomerMetricStore.Object, mockGlobalMetricValueStore.Object);

            var generator = MetricGenerator.ForAggregateCustomerMetric(metric, definition);
            await sut.RunAggregateCustomerMetricWorkflow(generator);


            mockHistoricCustomerMetricStore.Verify(
                _ => _.GetAggregateMetricValuesString(It.Is<Metric>(_ => _.Id == metric.Id), It.Is<int>(_ => _ == 0)), Times.Exactly(1));

            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));

            mockGlobalMetricValueStore.Verify(_ => _.Create(It.Is<GlobalMetricValue>(_ => _.NumericValue == latestValueToExpect)));
        }
    }
}