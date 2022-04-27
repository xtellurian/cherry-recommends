using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class CustomerWorkflowTests
    {
        [Fact]
        public async Task CreateOrUpdateMultiple_1of2Exists_Only1Created()
        {
            // arrange
            var dateTimeProvider = new SystemDateTimeProvider();
            var mockLogger = Utility.MockLogger<CustomerWorkflows>();
            var mockContext = Utility.MockStorageContext();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var mapStore = new Mock<ITrackedUserSystemMapStore>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();

            var mockBusinessWorkflow = new Mock<IBusinessWorkflow>();
            var mockEventIngestor = new Mock<IEventIngestor>();

            mockCustomerStore.Setup(_ => _.Context).Returns(mockContext.Object);

            var existingCustomerId = "existing";
            var newCustomerId = "new";

            var existingCustomer = new Customer(existingCustomerId);
            var newCustomer = new Customer(newCustomerId);
            mockCustomerStore
                .Setup(_ => _.ExistsFromCommonId(It.Is<string>(_ => _ == newCustomer.CommonId), It.Is<long?>(_ => _ == newCustomer.EnvironmentId)))
                .ReturnsAsync(false);
            mockCustomerStore
                .Setup(_ => _.ExistsFromCommonId(It.Is<string>(_ => _ == existingCustomer.CommonId), It.Is<long?>(_ => _ == existingCustomer.EnvironmentId)))
                .ReturnsAsync(true);

            mockCustomerStore
                .Setup(_ => _.Create(It.Is<Customer>(_ => _.CommonId == newCustomer.CommonId)))
                .ReturnsAsync(newCustomer);
            mockCustomerStore
                .Setup(_ => _.ReadFromCommonId(It.Is<string>(_ => _ == existingCustomer.CommonId),
                    It.Is<long?>(_ => _ == existingCustomer.EnvironmentId),
                    It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, ICollection<TrackedUserSystemMap>>>>()))
                .ReturnsAsync(existingCustomer);

            var sut = new CustomerWorkflows(
                mockContext.Object,
                mockLogger.Object,
                mockCustomerStore.Object,
                mapStore.Object,
                mockIntegratedSystemStore.Object,
                dateTimeProvider
            );

            // act
            var createdCustomers = await sut.CreateOrUpdate(new List<PendingCustomer>
            {
                new PendingCustomer(existingCustomerId),
                new PendingCustomer(newCustomerId)
            });


            Assert.Equal(2, createdCustomers.Count());
            Assert.Contains(existingCustomer, createdCustomers);
        }

        [Fact]
        public async Task CreateOrUpdateMultiple_UpdatesOnlyWhen_OverwriteExisting()
        {
            // arrange
            var dateTimeProvider = new SystemDateTimeProvider();
            var mockLogger = Utility.MockLogger<CustomerWorkflows>();
            var mockContext = Utility.MockStorageContext();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var mapStore = new Mock<ITrackedUserSystemMapStore>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();

            var mockBusinessWorkflow = new Mock<IBusinessWorkflow>();
            var mockEventIngestor = new Mock<IEventIngestor>();

            mockCustomerStore.Setup(_ => _.Context).Returns(mockContext.Object);

            var existingCustomer1Id = "existing";
            var existingCustomer2Id = "new";

            var existingCustomer1 = new Customer(existingCustomer1Id);
            var existingCustomer2 = new Customer(existingCustomer2Id);
            mockCustomerStore
                .Setup(_ => _.ExistsFromCommonId(It.Is<string>(_ => _ == existingCustomer1.CommonId), It.Is<long?>(_ => _ == existingCustomer1.EnvironmentId)))
                .ReturnsAsync(true);
            mockCustomerStore
                .Setup(_ => _.ExistsFromCommonId(It.Is<string>(_ => _ == existingCustomer2.CommonId), It.Is<long?>(_ => _ == existingCustomer2.EnvironmentId)))
                .ReturnsAsync(true);

            mockCustomerStore
                .Setup(_ => _.ReadFromCommonId(It.Is<string>(_ => _ == existingCustomer1.CommonId),
                    It.Is<long?>(_ => _ == existingCustomer1.EnvironmentId),
                    It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, ICollection<TrackedUserSystemMap>>>>()))
                .ReturnsAsync(existingCustomer1);
            mockCustomerStore
                .Setup(_ => _.ReadFromCommonId(It.Is<string>(_ => _ == existingCustomer2.CommonId),
                    It.Is<long?>(_ => _ == existingCustomer2.EnvironmentId),
                    It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, ICollection<TrackedUserSystemMap>>>>()))
                .ReturnsAsync(existingCustomer2);

            var sut = new CustomerWorkflows(
                mockContext.Object,
                mockLogger.Object,
                mockCustomerStore.Object,
                mapStore.Object,
                mockIntegratedSystemStore.Object,
                dateTimeProvider
            );

            // act
            var createdCustomers = await sut.CreateOrUpdate(new List<PendingCustomer>
            {
                new PendingCustomer(existingCustomer1Id, null, "UpdatedName", true),
                new PendingCustomer(existingCustomer2Id, null, "NotUpdatedName", false)
            });


            Assert.Equal(2, createdCustomers.Count());
            Assert.Equal(1, createdCustomers.Count(_ => _.Name == "UpdatedName"));
            Assert.Equal(0, createdCustomers.Count(_ => _.Name == "NotUpdatedName"));
        }
    }
}