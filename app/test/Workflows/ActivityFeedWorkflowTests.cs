using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class ActivityFeedWorkflowTests
    {
        [Fact]
        public async Task CanGetActivityFeedItems()
        {
            var mockLogger = Utility.MockLogger<ActivityFeedWorkflow>();
            var mockItemsRecommendationStore = new Mock<IItemsRecommendationStore>()
                .WithContext<Mock<IItemsRecommendationStore>, IItemsRecommendationStore, ItemsRecommendation>();
            var mockParameterSetRecommendationStore = new Mock<IParameterSetRecommendationStore>()
                .WithContext<Mock<IParameterSetRecommendationStore>, IParameterSetRecommendationStore, ParameterSetRecommendation>();
            var mockCustomerEventStore = new Mock<ICustomerEventStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            mockCustomerEventStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var customer = new Customer("customer1");
            var customerEvent = new CustomerEvent(customer, "eventId", DateTimeOffset.Now, null, EventKinds.Behaviour, "login", null);
            mockCustomerEventStore
                .Setup(_ => _.Latest(It.IsAny<IPaginate>()))
                .ReturnsAsync(new Paginated<CustomerEvent>(new List<CustomerEvent>() { customerEvent }, 1, 1, 1));

            mockItemsRecommendationStore
                .Setup(_ => _.Query(It.IsAny<EntityStoreQueryOptions<ItemsRecommendation>>()))
                .ReturnsAsync(new Paginated<ItemsRecommendation>(new List<ItemsRecommendation>(), 1, 0, 1));

            mockParameterSetRecommendationStore
                .Setup(_ => _.Query(It.IsAny<EntityStoreQueryOptions<ParameterSetRecommendation>>()))
                .ReturnsAsync(new Paginated<ParameterSetRecommendation>(new List<ParameterSetRecommendation>(), 1, 0, 1));

            PaginateRequest paginateReq = new PaginateRequest();
            var sut = new ActivityFeedWorkflow(mockLogger.Object, mockCustomerEventStore.Object, mockItemsRecommendationStore.Object, mockParameterSetRecommendationStore.Object);

            var result = await sut.GetActivityFeedEntities(paginateReq);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            mockCustomerEventStore.Verify(_ => _.Latest(It.IsAny<IPaginate>()), Times.Once);
            mockItemsRecommendationStore.Verify(_ => _.Query(It.IsAny<EntityStoreQueryOptions<ItemsRecommendation>>()), Times.Once);
            mockParameterSetRecommendationStore.Verify(_ => _.Query(It.IsAny<EntityStoreQueryOptions<ParameterSetRecommendation>>()), Times.Once);
        }


    }
}