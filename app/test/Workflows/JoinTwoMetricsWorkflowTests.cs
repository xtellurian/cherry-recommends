using System;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Metrics;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class JoinTwoMetricsWorkflowTests
    {

        [Theory]
        [InlineData(6, 2, 3)]
        [InlineData(6, 0, null)]
        [InlineData(2, 0.5, 4)]
        [InlineData(2, null, null)]
        [InlineData(null, null, null)]
        public async Task CanCalculateDivisionOfTwoMetrics(double? metric1Value, double? metric2Value, double? result)
        {
            var now = DateTime.Now;
            var mockStorageContext = new Mock<IStorageContext>();
            var mockGlobalMetricValueStore = new Mock<IGlobalMetricValueStore>();

            mockGlobalMetricValueStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var metric1 = new Metric
            {
                Id = 1,
                ValueType = MetricValueType.Numeric,
                Scope = MetricScopes.Global
            };
            var metric2 = new Metric
            {
                Id = 2,
                ValueType = MetricValueType.Numeric,
                Scope = MetricScopes.Global
            };
            var joinMetric = new Metric
            {
                Id = 3,
                ValueType = MetricValueType.Numeric,
                Scope = MetricScopes.Global,
            };
            var definition = new JoinTwoMetrics
            {
                Metric1 = metric1,
                Metric1Id = metric1.Id,
                Metric2 = metric2,
                Metric2Id = metric2.Id,
                JoinType = JoinType.Divide
            };

            var sut = new JoinTwoMetricsWorkflow(
                mockGlobalMetricValueStore.Object,
                Utility.MockLogger<JoinTwoMetricsWorkflow>().Object);

            // gets called 3 times
            if (metric1Value.HasValue)
            {
                mockGlobalMetricValueStore
                    .Setup(_ => _.LatestMetricValue(It.Is<Metric>(m => m.Id == metric1.Id)))
                    .ReturnsAsync(new GlobalMetricValue(metric1, 1, metric1Value.Value));
            }

            if (metric2Value.HasValue)
            {
                mockGlobalMetricValueStore
                    .Setup(_ => _.LatestMetricValue(It.Is<Metric>(m => m.Id == metric2.Id)))
                    .ReturnsAsync(new GlobalMetricValue(metric2, 1, metric2Value.Value));
            }

            mockGlobalMetricValueStore
                .Setup(_ => _.LatestMetricValue(It.Is<Metric>(m => m.Id == joinMetric.Id)))
                .ReturnsAsync(new GlobalMetricValue(joinMetric, 1, 1));

            var generator = MetricGenerator.CreateJoinTwoGlobalMetric(joinMetric, definition);
            await sut.RunJoinTwoMetricWorkflow(generator);

            if (result.HasValue)
            {
                mockGlobalMetricValueStore.Verify(_ => _.Create(It.Is<GlobalMetricValue>(m => m.NumericValue == result.Value)));

                mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
            }
            else
            {
                mockGlobalMetricValueStore.Verify(_ => _.Create(It.IsAny<GlobalMetricValue>()), Times.Never);
                mockStorageContext.Verify(_ => _.SaveChanges(), Times.Never);
            }

        }
    }
}