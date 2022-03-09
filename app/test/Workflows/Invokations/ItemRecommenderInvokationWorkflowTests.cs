using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Stores
{
#nullable enable
    public class ItemRecommenderInvokationWorkflowTests
    {

        [Theory]
        [InlineData(null, "business", PromotionRecommenderTargetTypes.Customer)]
        [InlineData("customer", null, PromotionRecommenderTargetTypes.Business)]
        public async Task InvokeForWrongTarget_Throws(string? customerId, string? businessId, PromotionRecommenderTargetTypes targetType)
        {

            var mockLogger = Utility.MockLogger<ItemsRecommenderInvokationWorkflows>();
            var mockStorageContext = new Mock<IStorageContext>();
            var dateTimeProvider = new SystemDateTimeProvider();
            var mockRecommendationCache = new Mock<IRecommendationCache<ItemsRecommender, ItemsRecommendation>>();
            var mockRecommenderModelClientFactory = new Mock<IRecommenderModelClientFactory>();
            var mockCustomerWorkflow = new Mock<ICustomerWorkflow>();
            var mockBusinessWorkflow = new Mock<IBusinessWorkflow>();
            var mockHistoricCustomerMetricStore = new Mock<IHistoricCustomerMetricStore>();
            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockWebhookSenderClient = new Mock<IWebhookSenderClient>();
            var mockCorrelatorStore = new Mock<IRecommendationCorrelatorStore>();
            var mockItemsRecommenderStore = new Mock<IItemsRecommenderStore>();
            var mockItemsRecommendationStore = new Mock<IItemsRecommendationStore>();

            var sut = new ItemsRecommenderInvokationWorkflows(
                mockLogger.Object,
                mockStorageContext.Object,
                dateTimeProvider,
                mockRecommendationCache.Object,
                mockRecommenderModelClientFactory.Object,
                mockCustomerWorkflow.Object,
                mockBusinessWorkflow.Object,
                mockHistoricCustomerMetricStore.Object,
                mockRecommendableItemStore.Object,
                mockWebhookSenderClient.Object,
                mockCorrelatorStore.Object,
                mockItemsRecommenderStore.Object,
                mockItemsRecommendationStore.Object
            );

            var baseline = new RecommendableItem("item1", "Item 1", 1, 1, BenefitType.Percent, 1, PromotionType.Discount, null);
            var other = new RecommendableItem("item2", "Item 2", 1, 1, BenefitType.Percent, 1, PromotionType.Discount, null);
            var allItems = new List<RecommendableItem> { baseline, other };
            var recommender = new ItemsRecommender("recommenderId", "Recomender", baseline, allItems, null, null, null)
            {
                TargetType = targetType
            };
            var input = new ItemsModelInputDto
            {
                CustomerId = customerId,
                BusinessId = businessId
            };
            // act
            await Assert.ThrowsAsync<BadRequestException>(() => sut.InvokeItemsRecommender(recommender, input));
        }
    }
}