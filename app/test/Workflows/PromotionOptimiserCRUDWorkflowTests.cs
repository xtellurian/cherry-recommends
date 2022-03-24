using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class PromotionOptimiserCRUDWorkflowTests
    {
        [Fact]
        public async Task CanCreate_HappyPath()
        {
            // arrange
            var baseline = EntityFactory.Promotion();
            var other = EntityFactory.Promotion();
            var promotions = new List<RecommendableItem>
            {
                baseline, other
            };
            var recommender = new ItemsRecommender("commonId",
                                                   "name",
                                                   baselineItem: baseline,
                                                   promotions,
                                                   arguments: null,
                                                   settings: null,
                                                   targetMetric: null,
                                                   numberOfItemsToRecommend: 1);

            var mockPromotionOptimiserStore = new Mock<IPromotionOptimiserStore>();
            var mockStorageContext = Utility.MockStorageContext();
            mockPromotionOptimiserStore.SetupStoreCreate<Mock<IPromotionOptimiserStore>, IPromotionOptimiserStore, PromotionOptimiser>();
            mockPromotionOptimiserStore.WithContext<Mock<IPromotionOptimiserStore>, IPromotionOptimiserStore, PromotionOptimiser>(mockStorageContext.Object);
            var mockItemsRecommenderStore = new Mock<IItemsRecommenderStore>();
            var sut = new PromotionOptimiserCRUDWorkflow(mockPromotionOptimiserStore.Object, mockItemsRecommenderStore.Object);

            // act
            var output = await sut.Create(recommender);

            // assert
            Assert.NotNull(output);
            Assert.Equal(2, output.Weights.Count);
            Assert.True(output.Weights.Sum(w => w.Weight) <= 1);
            Assert.Equal(recommender, output.Recommender);
            Assert.Equal(output, recommender.Optimiser);

            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
        }
    }
}