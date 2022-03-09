using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class CustomerSegmentWorkflowTests
    {
        [Theory]
        [InlineData("Australian")]
        [InlineData("Filipino")]
        [InlineData("Workforce")]
        public async Task CanCreateSegment(string name)
        {
            // arrange

            var mockLogger = Utility.MockLogger<CustomerSegmentWorkflows>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockSegmentStore = new Mock<ISegmentStore>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            mockSegmentStore.Setup(_ => _.Create(It.Is<Core.Segment>(x => x.Name == name))).ReturnsAsync(new Core.Segment(name));

            // act

            var workflow = new CustomerSegmentWorkflows(
                mockLogger.Object,
                mockSegmentStore.Object,
                mockCustomerStore.Object,
                mockStorageContext.Object);

            var segment = await workflow.CreateSegment(name);

            // assert

            Assert.Equal(name, segment.Name);

            mockSegmentStore.Verify(_ => _.Create(It.Is<Core.Segment>(x => x.Name == name)), Times.Exactly(1));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));
        }

        [Theory]
        [InlineData(1, "Accounting", 1, false)]
        [InlineData(2, "IT", 2, true)]
        [InlineData(3, "Sales", 3, false)]
        public async Task CanAddCustomerToSegment(long segmentId, string segmentName, long customerId, bool existsInSegment)
        {
            // arrange
            var segment = new Core.Segment(segmentName);
            segment.Id = segmentId;

            var customer = new Customer($"commonId{customerId}");
            customer.Id = customerId;

            var mockLogger = Utility.MockLogger<CustomerSegmentWorkflows>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockSegmentStore = new Mock<ISegmentStore>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            mockSegmentStore.Setup(_ => _.ExistsInSegment(It.Is<long>(x => x == segment.Id), It.Is<long>(x => x == customer.Id))).ReturnsAsync(existsInSegment);
            mockSegmentStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockSegmentStore.Setup(_ => _.AddCustomer(It.Is<Core.Segment>(_ => _.Id == segment.Id), It.Is<Customer>(_ => _.Id == customer.Id)))
                .ReturnsAsync(new CustomerSegment(customer, segment));

            // act

            var workflow = new CustomerSegmentWorkflows(
                mockLogger.Object,
                mockSegmentStore.Object,
                mockCustomerStore.Object,
                mockStorageContext.Object);

            await workflow.AddToSegment(segment, customer);

            // assert

            mockSegmentStore.Verify(_ => _.AddCustomer(It.Is<Core.Segment>(_ => _.Id == segment.Id), It.Is<Customer>(_ => _.Id == customer.Id)), Times.Exactly(1));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));
        }

        [Theory]
        [InlineData(1, "Accounting", 1, false)]
        [InlineData(2, "IT", 2, true)]
        [InlineData(3, "Sales", 3, false)]
        public async Task CanRemoveCustomerFromSegment(long segmentId, string segmentName, long customerId, bool existsInSegment)
        {
            // arrange
            var segment = new Core.Segment(segmentName);
            segment.Id = segmentId;

            var customer = new Customer($"commonId{customerId}");
            customer.Id = customerId;

            var mockLogger = Utility.MockLogger<CustomerSegmentWorkflows>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockSegmentStore = new Mock<ISegmentStore>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            mockSegmentStore.Setup(_ => _.ExistsInSegment(It.Is<long>(x => x == segment.Id), It.Is<long>(x => x == customer.Id))).ReturnsAsync(existsInSegment);
            mockSegmentStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockSegmentStore.Setup(_ => _.RemoveCustomer(It.Is<Core.Segment>(_ => _.Id == segment.Id), It.Is<Customer>(_ => _.Id == customer.Id)))
                .ReturnsAsync(new CustomerSegment(customer, segment));

            // act

            var workflow = new CustomerSegmentWorkflows(
                mockLogger.Object,
                mockSegmentStore.Object,
                mockCustomerStore.Object,
                mockStorageContext.Object);

            await workflow.RemoveFromSegment(segment, customer);

            // assert

            mockSegmentStore.Verify(_ => _.RemoveCustomer(It.Is<Core.Segment>(_ => _.Id == segment.Id), It.Is<Customer>(_ => _.Id == customer.Id)), Times.Exactly(1));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));
        }
    }
}