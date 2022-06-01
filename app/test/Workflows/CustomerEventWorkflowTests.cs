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
    public class CustomerEventWorkflowTests
    {
        [Fact]
        public async Task IdentifyEvent_AddsTo_CustomerProperties()
        {
            var dateTimeProvider = new SystemDateTimeProvider();
            var mockLogger = Utility.MockLogger<CustomerEventsWorkflows>();
            var mockTelemetry = Utility.MockTelemetry();
            var mockContext = Utility.MockStorageContext();
            var mockCustomerWorkflow = new Mock<ICustomerWorkflow>();
            var mockEnvironmentProvider = new Mock<IEnvironmentProvider>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockCustomerEventStore = new Mock<ICustomerEventStore>();
            var mockBusinessWorkflow = new Mock<IBusinessWorkflow>();
            var mockEventIngestor = new Mock<ICustomerEventIngestor>();
            var mockOfferWorkflow = new Mock<IOfferWorkflow>();

            mockCustomerEventStore.Setup(_ => _.Context).Returns(mockContext.Object);
            mockCustomerEventStore.Setup(_ => _.AddRange(It.Is<IEnumerable<CustomerEvent>>(x => x.Count() == 1)))
                .ReturnsAsync((IEnumerable<CustomerEvent> e) => e);

            var tenantName = "tenant";
            var customerId = "customer";
            var customerEmail = "test@example.org";
            var customerFirstName = "Barry";
            var properties = new Dictionary<string, object>
            {
                { "email", customerEmail },
                { "firstName", customerFirstName },
                { "age", 26 },
            };

            var customer = new Customer(customerId);
            mockCustomerWorkflow
                .Setup(_ => _.CreateOrUpdate(It.IsAny<IEnumerable<PendingCustomer>>(), It.IsAny<bool>()))
                .ReturnsAsync(new List<Customer> { customer });

            var sut = new CustomerEventsWorkflows(
                dateTimeProvider,
                mockTelemetry.Object,
                mockLogger.Object,
                mockCustomerWorkflow.Object,
                mockEnvironmentProvider.Object,
                mockIntegratedSystemStore.Object,
                mockCustomerEventStore.Object,
                mockBusinessWorkflow.Object,
                mockEventIngestor.Object,
                mockOfferWorkflow.Object
            );

            var response = await sut.ProcessEvents(new List<CustomerEventInput>
            {
                new CustomerEventInput(tenantName, customerId, null, "eventId", null, null, null, null, EventKinds.Identify, "eventType", properties )
            });

            Assert.Equal(1, response.EventsProcessed);
            Assert.Equal(customerEmail, customer.Email);
            Assert.Equal(customerFirstName, customer.Name);
        }
    }
}