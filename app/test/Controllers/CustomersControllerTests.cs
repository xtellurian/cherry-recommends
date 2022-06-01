using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Controllers;
using SignalBox.Web.Dto;
using Xunit;

namespace SignalBox.Test.Controllers
{
    public class CustomersControllerTests
    {
        private static CustomersController CreateCustomersController(Customer customer)
        {
            var dateTimeProvider = Utility.DateTimeProvider();
            var mockWorkflowLogger = Utility.MockLogger<CustomerWorkflows>();
            var mockContext = Utility.MockStorageContext();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var mockCustomerEventStore = new Mock<ICustomerEventStore>();
            var mockSegmentStore = new Mock<ISegmentStore>();
            var mockTrackedUserSystemMapStore = new Mock<ITrackedUserSystemMapStore>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockLogger = Utility.MockLogger<CustomersController>();
            var mockIngestor = new Mock<ICustomerHasUpdatedIngestor>();
            var mockTenantProvider = new Mock<ITenantProvider>();

            var customerWorkflow = new CustomerWorkflows(mockContext.Object,
                mockWorkflowLogger.Object,
                mockCustomerStore.Object,
                mockTrackedUserSystemMapStore.Object,
                mockIntegratedSystemStore.Object,
                mockIngestor.Object,
                dateTimeProvider,
                mockTenantProvider.Object);

            var controller = new CustomersController(mockLogger.Object,
                                      dateTimeProvider,
                                      customerWorkflow,
                                      mockCustomerStore.Object,
                                      mockCustomerEventStore.Object,
                                      mockSegmentStore.Object);

            mockCustomerStore.SetupCommonStoreRead<Mock<ICustomerStore>, ICustomerStore, Customer>(customer);

            mockCustomerStore
                .Setup(_ => _.Create(It.Is<Customer>(_ => _.CommonId == customer.CommonId)))
                .ReturnsAsync(customer);

            var customerEvent = new CustomerEvent(customer, "event1", DateTime.Now, null, EventKinds.ConsumeRecommendation, "eventType", null);
            var events = new List<CustomerEvent>() { customerEvent };
            mockCustomerEventStore
                .Setup(_ => _.ReadEventsForUser(It.IsAny<IPaginate>(), It.Is<Customer>(_ => _.CustomerId == customer.CustomerId), It.IsAny<Expression<Func<CustomerEvent, bool>>>()))
                .ReturnsAsync(new Paginated<CustomerEvent>(events, 1, events.Count, 1));

            return controller;
        }

        [Fact]
        public async Task CanGetResource()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);

            var sut = CreateCustomersController(customer);
            var result = await sut.GetResource("customer1");

            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
        }

        [Fact]
        public async Task CanGetEvents()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);
            PaginateRequest paginate = new PaginateRequest();

            var sut = CreateCustomersController(customer);
            var result = await sut.GetEvents(paginate, "customer1");

            Assert.NotNull(result);
            Assert.IsType<Paginated<CustomerEvent>>(result);
            Assert.NotNull(result.Items);

        }

        [Fact]
        public async Task CanGetSegments()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);

            var sut = CreateCustomersController(customer);
            var result = await sut.GetSegments("customer1");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<SignalBox.Core.Segment>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task CanSetProperties()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);

            var properties = new DynamicPropertyDictionary();
            properties.Add("email", "test@gmail.com");
            properties.Add("firstName", "Sam");

            var sut = CreateCustomersController(customer);
            var result = await sut.SetProperties("customer1", properties);

            Assert.NotNull(result);
            Assert.IsType<DynamicPropertyDictionary>(result);
            Assert.Equal(properties.Count, result.Count);
        }

        [Fact]
        public async Task CanCreateOrUpdate()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);

            var dto = new CreateOrUpdateCustomerDto();
            dto.CustomerId = customerId;
            dto.Name = "John";
            dto.Email = "john@testing.com";
            dto.Properties = new Dictionary<string, object>() { { "age", 24 } };
            dto.IntegratedSystemReference = new IntegratedSystemReference();

            var sut = CreateCustomersController(customer);
            var result = await sut.CreateOrUpdate(dto);

            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
        }

        [Fact]
        public async Task CanCreateBatch_Customers()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);

            var customerDto = new CreateOrUpdateCustomerDto();
            customerDto.CustomerId = customerId;
            customerDto.Name = "John";
            customerDto.Email = "john@testing.com";
            customerDto.Properties = new Dictionary<string, object>() { { "age", 24 } };
            customerDto.IntegratedSystemReference = new IntegratedSystemReference();

            var batchDto = new BatchCreateOrUpdateCustomersDto();
            batchDto.Customers = new List<CreateOrUpdateCustomerDto>() { customerDto };

            var sut = CreateCustomersController(customer);
            var result = await sut.CreateBatch(batchDto);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CanCreateBatch_Users()
        {
            string customerId = "customer1";
            var customer = new Customer(customerId);

            var customerDto = new CreateOrUpdateCustomerDto();
            customerDto.CustomerId = customerId;
            customerDto.Name = "John";
            customerDto.Email = "john@testing.com";
            customerDto.Properties = null;
            customerDto.IntegratedSystemReference = null;

            var batchDto = new BatchCreateOrUpdateCustomersDto();
            batchDto.Users = new List<CreateOrUpdateCustomerDto>() { customerDto };

            var sut = CreateCustomersController(customer);
            var result = await sut.CreateBatch(batchDto);

            Assert.NotNull(result);
        }
    }
}