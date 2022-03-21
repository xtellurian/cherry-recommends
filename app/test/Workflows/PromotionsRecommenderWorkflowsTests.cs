using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class PromotionsRecommenderWorkflowsTests
    {
        [Fact]
        public async Task CanCreate_HappyPath()
        {

            // arrange
            var mockPromoRecommenderStore = new Mock<IItemsRecommenderStore>()
                .WithContext<Mock<IItemsRecommenderStore>, IItemsRecommenderStore, ItemsRecommender>();
            var mockPromoRecommendationStore = new Mock<IItemsRecommendationStore>();
            var mockSegmentStore = new Mock<ISegmentStore>();
            var mockMetricStore = new Mock<IMetricStore>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockCategoricalOptimiserClient = new Mock<ICategoricalOptimiserClient>();
            var mockModelRegistrationStore = new Mock<IModelRegistrationStore>();
            var mockAudienceStore = new Mock<IAudienceStore>();
            var mockReportWorkflow = new Mock<IRecommenderReportImageWorkflow>();
            var mockPromoStore = new Mock<IRecommendableItemStore>()
                .WithContext<Mock<IRecommendableItemStore>, IRecommendableItemStore, RecommendableItem>();

            var baselinePromo = new RecommendableItem("item1", "Item 1", null, 1, BenefitType.Percent, 0.2, PromotionType.Discount, null)
                .WithId();
            var promo2 = new RecommendableItem("item2", "Item 2", null, 1, BenefitType.Percent, 0.3, PromotionType.Discount, null)
                .WithId();
            var promo3 = new RecommendableItem("item3", "Item 3", null, 1, BenefitType.Percent, 0.4, PromotionType.Discount, null)
                .WithId();
            mockPromoStore.SetupCommonStoreRead<Mock<IRecommendableItemStore>, IRecommendableItemStore, RecommendableItem>(baselinePromo, promo2, promo3);

            mockPromoRecommenderStore.Setup(_ => _.Create(It.IsAny<ItemsRecommender>())).ReturnsAsync((ItemsRecommender r) => r);

            var sut = new PromotionsRecommenderWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockSegmentStore.Object,
                mockIntegratedSystemStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object

            );

            // act
            var output = await sut.CreateItemsRecommender(
                new CreateCommonEntityModel("commonId", "Name"),
                baselineItemId: baselinePromo.CommonId,
                itemIds: new List<string> { promo2.CommonId, promo3.CommonId },
                segmentIds: null,
                numberOfItemsToRecommend: 1,
                arguments: null,
                settings: new RecommenderSettings(),
                useOptimiser: false,
                targetMetricId: null,
                targetType: PromotionRecommenderTargetTypes.Customer,
                useInternalId: null
            );

            // assert
            Assert.NotNull(output);
            Assert.Equal("commonId", output.CommonId);
            Assert.Equal(baselinePromo.Id, output.BaselineItemId);
            Assert.Equal(3, output.Items.Count); // 2 + baseline
            Assert.Contains(baselinePromo, output.Items);
        }
    }
}