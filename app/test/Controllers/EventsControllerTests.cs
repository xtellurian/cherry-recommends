using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Controllers;
using SignalBox.Web.Dto;
using Xunit;

namespace SignalBox.Test.Controllers
{
    public class EventsControllerTests
    {
        private static EventsController CreateEventsController(string customerId, string eventId)
        {
            var dateTimeProvider = Utility.DateTimeProvider();
            var mockContext = Utility.MockStorageContext();
            var mockCustomerEventStore = new Mock<ICustomerEventStore>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockEnvironmentProvider = new Mock<IEnvironmentProvider>();
            var mockTenanProvider = new Mock<ITenantProvider>();
            var mockLogger = Utility.MockLogger<CustomerEventsWorkflows>();
            var mockTelemetry = Utility.MockTelemetry();
            var mockCustomerWorkflow = new Mock<ICustomerWorkflow>();
            var mockBusinessWorkflow = new Mock<IBusinessWorkflow>();
            var mockEventIngestor = new Mock<ICustomerEventIngestor>();
            var mockOfferWorkflow = new Mock<IOfferWorkflow>();

            Customer customer = new(customerId);
            var customerEvent = new CustomerEvent(customer, eventId, DateTime.Now, null, EventKinds.ConsumeRecommendation, "eventType", null);

            mockCustomerEventStore.Setup(_ => _.Context).Returns(mockContext.Object);
            mockCustomerEventStore.Setup(_ => _.Read(It.Is<string>(x => x == customerEvent.EventId))).ReturnsAsync(customerEvent);
            mockCustomerEventStore.Setup(_ => _.AddRange(It.Is<IEnumerable<CustomerEvent>>(x => x.Count() == 1)))
                .ReturnsAsync((IEnumerable<CustomerEvent> e) => e);
            mockCustomerWorkflow
                 .Setup(_ => _.CreateOrUpdate(It.IsAny<IEnumerable<PendingCustomer>>(), It.IsAny<bool>()))
                 .ReturnsAsync(new List<Customer> { customer });

            var customerEventsWorkflow = new CustomerEventsWorkflows(
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

            var controller = new EventsController(mockEnvironmentProvider.Object,
                                    mockTenanProvider.Object,
                                    customerEventsWorkflow,
                                    mockCustomerEventStore.Object);

            return controller;
        }

        [Theory]
        [InlineData(EventKinds.AddToBusiness)]
        [InlineData(EventKinds.Behaviour)]
        [InlineData(EventKinds.ConsumeRecommendation)]
        [InlineData(EventKinds.Custom)]
        [InlineData(EventKinds.Identify)]
        [InlineData(EventKinds.PageView)]
        [InlineData(EventKinds.PropertyUpdate)]
        [InlineData(EventKinds.Purchase)]
        [InlineData(EventKinds.UsePromotion)]
        public async Task CanLogEvent(EventKinds eventKind)
        {
            string customerId = "customer1";
            string eventId = "event1";
            var sut = CreateEventsController(customerId, eventId);

            EventDto dto = new()
            {
                CommonUserId = customerId,
                CustomerId = customerId,
                EventId = eventId,
                Timestamp = DateTime.Now,
                RecommendationCorrelatorId = 1,
                SourceSystemId = 1,
                Kind = eventKind,
                EventType = "PageView",
                Properties = new EventProperties { { "puchase", 50 } }
            };

            var result = await sut.LogEvents(new List<EventDto>() { dto });

            Assert.NotNull(result);
            Assert.IsType<EventLoggingResponse>(result);
        }

        [Fact]
        public async Task CanGetEvent()
        {
            string customerId = "customer1";
            string eventId = "event1";

            var sut = CreateEventsController(customerId, eventId);
            var result = await sut.GetEvent(eventId);

            Assert.NotNull(result);
            Assert.IsType<CustomerEvent>(result);
        }
    }
}