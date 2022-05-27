using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class RecommendableItemWorkflowsTests
    {
        [Fact]
        public async Task CanCreateRecommendableItem()
        {
            // Arrange

            var recommendableItem = new RecommendableItem("test-123", "test", 1,
                1, BenefitType.Fixed, 3, PromotionType.Other, null);

            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockItemsRecommenderStore = new Mock<IPromotionsCampaignStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            mockRecommendableItemStore.Setup(x => x.Create(It.Is<RecommendableItem>(_ => _.CommonId == recommendableItem.CommonId)))
                .ReturnsAsync(recommendableItem);
            mockRecommendableItemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockItemsRecommenderStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var sut = new RecommendableItemWorkflows(mockRecommendableItemStore.Object,
                mockItemsRecommenderStore.Object, mockStorageContext.Object);

            // Act

            var created = await sut.CreateRecommendableItem(recommendableItem.CommonId, recommendableItem.Name, recommendableItem.DirectCost,
                recommendableItem.NumberOfRedemptions, recommendableItem.BenefitType, recommendableItem.BenefitValue,
                recommendableItem.PromotionType, recommendableItem.Description, recommendableItem.Properties);

            // Assert

            mockRecommendableItemStore.Verify(_ => _.Create(It.Is<RecommendableItem>(_ => _.CommonId == recommendableItem.CommonId)), Times.Exactly(1));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));
        }

        [Fact]
        public async Task CanUpdateRecommendableItem()
        {
            // Arrange

            var recommendableItem = new RecommendableItem("test-123", "test", 1,
                1, BenefitType.Fixed, 3, PromotionType.Other, null);

            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockItemsRecommenderStore = new Mock<IPromotionsCampaignStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            mockRecommendableItemStore.Setup(x => x.Create(It.Is<RecommendableItem>(_ => _.CommonId == recommendableItem.CommonId)))
                .ReturnsAsync(recommendableItem);
            mockRecommendableItemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockItemsRecommenderStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var sut = new RecommendableItemWorkflows(mockRecommendableItemStore.Object,
                mockItemsRecommenderStore.Object, mockStorageContext.Object);

            string uName = "updated name";
            double? uDirectCost = 5;
            int uNumberOfRedemptions = 3;
            BenefitType uBenefitType = BenefitType.Percent;
            double uBenefitValue = 100;
            PromotionType uPromotionType = PromotionType.Upgrade;
            string uDescription = "updated description";
            DynamicPropertyDictionary uProperties = new DynamicPropertyDictionary();
            uProperties.Add("prop1", 1);

            // Act

            var updated = await sut.UpdateRecommendableItem(recommendableItem, uName, uDirectCost, uNumberOfRedemptions,
                uBenefitType, uBenefitValue, uPromotionType, uDescription, uProperties);

            // Assert

            Assert.Equal(uName, updated.Name);
            Assert.Equal(uDirectCost, updated.DirectCost);
            Assert.Equal(uNumberOfRedemptions, updated.NumberOfRedemptions);
            Assert.Equal(uBenefitType, updated.BenefitType);
            Assert.Equal(uBenefitValue, updated.BenefitValue);
            Assert.Equal(uPromotionType, updated.PromotionType);
            Assert.Equal(uDescription, updated.Description);
            Assert.Equal(uProperties, updated.Properties);

            mockRecommendableItemStore.Verify(_ => _.Update(It.Is<RecommendableItem>(_ => _.CommonId == recommendableItem.CommonId)), Times.Exactly(1));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(1));
        }

        [Fact]
        public async Task CanDetermineIsBaselineItemForRecommender()
        {
            // Arrange

            var recommendableItem = new RecommendableItem("test-123", "test", 1,
                1, BenefitType.Fixed, 3, PromotionType.Other, null);

            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockItemsRecommenderStore = new Mock<IPromotionsCampaignStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            mockRecommendableItemStore.Setup(x => x.Create(It.Is<RecommendableItem>(_ => _.CommonId == recommendableItem.CommonId)))
                .ReturnsAsync(recommendableItem);
            mockRecommendableItemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockItemsRecommenderStore.Setup(x => x.Count(_ => _.BaselineItemId == recommendableItem.Id)).ReturnsAsync(1);
            mockItemsRecommenderStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var sut = new RecommendableItemWorkflows(mockRecommendableItemStore.Object,
                mockItemsRecommenderStore.Object, mockStorageContext.Object);

            // Act

            var isBaselineItemForRecommender = await sut.IsBaselineItemForRecommender(recommendableItem);

            // Assert

            Assert.True(isBaselineItemForRecommender);

            mockItemsRecommenderStore.Verify(_ => _.Count(_ => _.BaselineItemId == recommendableItem.Id), Times.Exactly(1));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Exactly(0));
        }
    }
}