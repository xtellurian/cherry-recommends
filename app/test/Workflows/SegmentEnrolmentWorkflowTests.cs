using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Predicates;
using SignalBox.Core.Segments;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class SegmentEnrolmentWorkflowTests
    {
        [Fact]
        public async Task RunAllEnrolments_NoEnrolments_NoError()
        {
            // arrange

            var mockLogger = Utility.MockLogger<SegmentEnrolmentWorkflow>();
            var mockEnrolmentRuleStore = new Mock<IEnrolmentRuleStore>();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var mockHistoricCustomerMetricStore = new Mock<IHistoricCustomerMetricStore>();
            var mockCustomerSegmentWorkflow = new Mock<ICustomerSegmentWorkflow>();
            var dtProvider = Utility.DateTimeProvider();

            mockEnrolmentRuleStore.Setup(_ => _.Iterate(It.IsAny<Expression<Func<EnrolmentRule, bool>>>(), It.IsAny<IterateOrderBy>()))
                .Returns(new List<MetricEnrolmentRule>().ToAsyncEnumerable()); // empty list

            // act

            var sut = new SegmentEnrolmentWorkflow(
                mockEnrolmentRuleStore.Object,
                mockCustomerSegmentWorkflow.Object,
                mockCustomerStore.Object,
                mockHistoricCustomerMetricStore.Object,
                dtProvider,
                mockLogger.Object);


            await sut.RunAllEnrolmentRules();

            // assert - nothing
        }

        [Fact]
        public async Task RunAllEnrolments_MetricEnrolments()
        {
            // arrange
            var mockLogger = Utility.MockLogger<SegmentEnrolmentWorkflow>();
            var mockEnrolmentRuleStore = new Mock<IEnrolmentRuleStore>();
            mockEnrolmentRuleStore.SetupContext<IEnrolmentRuleStore, EnrolmentRule>();
            var mockCustomerSegmentWorkflow = new Mock<ICustomerSegmentWorkflow>();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var mockHistoricCustomerMetricStore = new Mock<IHistoricCustomerMetricStore>();
            var dtProvider = Utility.DateTimeProvider();
            var customerToBeInSegment = new Customer("customerInSegment")
            {
                Id = 1
            };
            var customerNotToBeInSegment = new Customer("customerNotInSegment")
            {
                Id = 2
            };
            var metric = new Metric
            {
                Id = 1235
            };

            var customers = new List<Customer> { customerToBeInSegment, customerNotToBeInSegment };

            mockCustomerStore.Setup(_ => _.Read(It.IsAny<long>()))
                .ReturnsAsync((long id) => customers.First(c => c.Id == id)); // return the requested customer
            mockHistoricCustomerMetricStore.Setup(_ => _.IterateLatest(It.Is<long>(m => m == metric.Id), It.IsAny<Expression<Func<LatestMetric, bool>>>()))
            .Returns(new List<LatestMetric>()
            {
                new LatestMetric
                {
                    CustomerId = customerToBeInSegment.Id,
                    MetricId =  metric.Id,
                }
            }.ToAsyncEnumerable());


            var segment = new Core.Segment("SegmentName");
            var rules = new List<MetricEnrolmentRule>
            {
                new MetricEnrolmentRule
                {
                    Metric = metric,
                    MetricId = metric.Id,
                    Segment = segment,
                    SegmentId = segment.Id,
                    NumericPredicate = new NumericPredicate(NumericPredicateOperators.None, 0)
                }
            };
            mockEnrolmentRuleStore.Setup(_ => _.Iterate(It.IsAny<Expression<Func<EnrolmentRule, bool>>>(), It.IsAny<IterateOrderBy>()))
                .Returns(rules.ToAsyncEnumerable());

            // act

            var sut = new SegmentEnrolmentWorkflow(
                mockEnrolmentRuleStore.Object,
                mockCustomerSegmentWorkflow.Object,
                mockCustomerStore.Object,
                mockHistoricCustomerMetricStore.Object,
                dtProvider,
                mockLogger.Object);

            await sut.RunAllEnrolmentRules();

            // assert
            mockCustomerSegmentWorkflow.Verify(
                _ => _.AddToSegment(It.Is<Core.Segment>(s => s == segment), It.Is<Customer>(c => c == customerToBeInSegment)), Times.Once);
            mockCustomerSegmentWorkflow.Verify(
                _ => _.AddToSegment(It.Is<Core.Segment>(s => s == segment), It.Is<Customer>(c => c == customerNotToBeInSegment)), Times.Never);
            Assert.Equal(dtProvider.Value, rules.First().LastCompleted);
        }

    }
}