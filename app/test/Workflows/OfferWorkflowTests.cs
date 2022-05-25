using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class OfferWorkflowTests
    {
        [Theory]
        [InlineData(EventKinds.AddToBusiness, 1, 1)]
        [InlineData(EventKinds.Behaviour, 2, 2)]
        [InlineData(EventKinds.ConsumeRecommendation, 3, 3)]
        [InlineData(EventKinds.Custom, 4, 4)]
        [InlineData(EventKinds.Identify, 5, 5)]
        [InlineData(EventKinds.PageView, 6, 6)]
        [InlineData(EventKinds.PropertyUpdate, 7, 7)]
        [InlineData(EventKinds.Purchase, null, 8)]
        [InlineData(EventKinds.UsePromotion, null, 9)]
        public async Task UpdateOffer_Returns_OnInvalidEvents(EventKinds eventKind, long? recommendationCorrelatorId, long? promotionId)
        {
            // Arrange
            var mockStorageContext = Utility.MockStorageContext();
            var mockOfferStore = new Mock<IOfferStore>();
            var mockPromotionStore = new Mock<IRecommendableItemStore>();
            var mockPromotionsRecommendationStore = new Mock<IItemsRecommendationStore>();
            var customer = new Customer("customer-id", "John Smith");
            var source = new IntegratedSystem("test-source", "Test Source", IntegratedSystemTypes.Segment);
            var properties = new Dictionary<string, object>()
            {
                { "promotionId", promotionId }
            };
            var customerEvent = new CustomerEvent(
                customer, "test-event", DateTimeOffset.Now, source,
                eventKind, "test-event", properties, recommendationCorrelatorId);

            mockOfferStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            // Act
            var workflow = new OfferWorkflows(
                Utility.MockLogger<OfferWorkflows>().Object,
                mockOfferStore.Object,
                mockPromotionStore.Object,
                mockPromotionsRecommendationStore.Object,
                Utility.DateTimeProvider()
            );
            await workflow.UpdateOffer(customerEvent);
            // Assert
            mockOfferStore.Verify(_ => _.ReadOfferByRecommendationCorrelator(It.IsAny<long>()), Times.Never);
            mockPromotionStore.Verify(_ => _.Read(It.IsAny<long>()), Times.Never);
            mockOfferStore.Verify(_ => _.Update(It.IsAny<Offer>()), Times.Never);
            mockOfferStore.Verify(_ => _.Context.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData(EventKinds.PromotionPresented, 1)]
        public async Task UpdateOffer_OfferIsPresented(EventKinds eventKind, long recommendationCorrelatorId)
        {
            // Arrange
            var mockStorageContext = Utility.MockStorageContext();
            var mockOfferStore = new Mock<IOfferStore>();
            var mockPromotionStore = new Mock<IRecommendableItemStore>();
            var mockPromotionsRecommendationStore = new Mock<IItemsRecommendationStore>();
            var dateTimeProvider = Utility.DateTimeProvider();
            var customer = new Customer("customer-id", "John Smith");
            var source = new IntegratedSystem("test-source", "Test Source", IntegratedSystemTypes.Segment);
            var properties = new Dictionary<string, object>();
            var customerEvent = new CustomerEvent(
                customer, "test-event", DateTimeOffset.Now, source,
                eventKind, "test-event", properties, recommendationCorrelatorId);
            var recommender = new ItemsRecommender(
                "test-recommender", "Test Recommender", RecommendableItem.DefaultRecommendableItem,
                new RecommendableItem[] { }, null, null, null);
            var recommendingContext = new RecommendingContext(
                new RecommendationCorrelator(recommender), null, null
            );
            var recommendation = new ItemsRecommendation(
                recommender,
                recommendingContext,
                new ScoredRecommendableItem[] { });
            var offerCreated = new Offer(recommendation);
            var offerPresented = new Offer(recommendation)
            {
                State = OfferState.Presented
            };
            var offerRedeemed = new Offer(recommendation)
            {
                State = OfferState.Redeemed,
                RedeemedAt = dateTimeProvider.Now
            };

            mockOfferStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockOfferStore
                .SetupSequence(_ => _.ReadOfferByRecommendationCorrelator(It.Is<long>(_ => _ == recommendationCorrelatorId)))
                .ReturnsAsync(new EntityResult<Offer>(offerCreated))
                .ReturnsAsync(new EntityResult<Offer>(offerPresented))
                .ReturnsAsync(new EntityResult<Offer>(offerRedeemed));
            // Act
            var workflow = new OfferWorkflows(
                Utility.MockLogger<OfferWorkflows>().Object,
                mockOfferStore.Object,
                mockPromotionStore.Object,
                mockPromotionsRecommendationStore.Object,
                dateTimeProvider
            );
            await workflow.UpdateOffer(customerEvent); // offerCreated
            await workflow.UpdateOffer(customerEvent); // offerPresented
            await workflow.UpdateOffer(customerEvent); // offerRedeemed
            // Assert
            mockOfferStore.Verify(_ => _.ReadOfferByRecommendationCorrelator(It.IsAny<long>()), Times.Exactly(3));
            mockPromotionStore.Verify(_ => _.Read(It.IsAny<long>()), Times.Never);
            mockOfferStore.Verify(_ => _.Update(It.Is<Offer>(_ => _.State == OfferState.Created)), Times.Never);
            mockOfferStore.Verify(_ => _.Update(It.Is<Offer>(_ => _.State == OfferState.Presented)), Times.Exactly(2));
            mockOfferStore.Verify(_ => _.Update(It.Is<Offer>(_ => _.State == OfferState.Redeemed)), Times.Once);
            mockOfferStore.Verify(_ => _.Context.SaveChanges(), Times.Never);
        }

        [Theory]
        [InlineData(EventKinds.Purchase, 1, 1)]
        [InlineData(EventKinds.UsePromotion, 2, 2)]
        public async Task UpdateOffer_OfferIsRedeemed(EventKinds eventKind, long recommendationCorrelatorId, long promotionId)
        {
            // Arrange
            var mockStorageContext = Utility.MockStorageContext();
            var mockOfferStore = new Mock<IOfferStore>();
            var mockPromotionStore = new Mock<IRecommendableItemStore>();
            var mockPromotionsRecommendationStore = new Mock<IItemsRecommendationStore>();
            var dateTimeProvider = Utility.DateTimeProvider();
            var customer = new Customer("customer-id", "John Smith");
            var source = new IntegratedSystem("test-source", "Test Source", IntegratedSystemTypes.Segment);
            var properties = new Dictionary<string, object>()
            {
                { "promotionId", promotionId }
            };
            var defaultValue = 10;
            if (eventKind == EventKinds.Purchase)
            {
                properties.Add("value", defaultValue);
            }
            var promotion = new RecommendableItem("test-promotion", "Test Promotion", 0, 1, BenefitType.Fixed, 1, PromotionType.Other, null)
            {
                Id = promotionId
            };
            var customerEvent = new CustomerEvent(
                customer, "test-event", DateTimeOffset.Now, source,
                eventKind, "test-event", properties, recommendationCorrelatorId);
            var recommender = new ItemsRecommender(
                "test-recommender", "Test Recommender", RecommendableItem.DefaultRecommendableItem,
                new RecommendableItem[] { promotion }, null, null, null);
            var recommendingContext = new RecommendingContext(
                new RecommendationCorrelator(recommender), null, null
            );
            var recommendation = new ItemsRecommendation(
                recommender,
                recommendingContext,
                new ScoredRecommendableItem[] { new ScoredRecommendableItem(promotion, 0.25, null) });
            var offerCreated = new Offer(recommendation);
            var offerPresented = new Offer(recommendation)
            {
                State = OfferState.Presented
            };
            var offerRedeemed = new Offer(recommendation)
            {
                State = OfferState.Redeemed,
                RedeemedAt = dateTimeProvider.Now
            };

            mockOfferStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockOfferStore
                .SetupSequence(_ => _.ReadOfferByRecommendationCorrelator(It.Is<long>(_ => _ == recommendationCorrelatorId)))
                .ReturnsAsync(new EntityResult<Offer>(offerCreated))
                .ReturnsAsync(new EntityResult<Offer>(offerPresented))
                .ReturnsAsync(new EntityResult<Offer>(offerRedeemed));
            mockPromotionStore.Setup(_ => _.Read(It.Is<long>(_ => _ == promotionId))).ReturnsAsync(promotion);
            // Act
            var workflow = new OfferWorkflows(
                Utility.MockLogger<OfferWorkflows>().Object,
                mockOfferStore.Object,
                mockPromotionStore.Object,
                mockPromotionsRecommendationStore.Object,
                dateTimeProvider
            );
            await workflow.UpdateOffer(customerEvent); // offerCreated
            await workflow.UpdateOffer(customerEvent); // offerPresented
            await workflow.UpdateOffer(customerEvent); // offerRedeemed
            // Assert
            mockOfferStore.Verify(_ => _.ReadOfferByRecommendationCorrelator(It.IsAny<long>()), Times.Exactly(3));
            mockPromotionStore.Verify(_ => _.Read(It.IsAny<long>()), Times.Exactly(2));
            mockOfferStore.Verify(_ => _.Update(It.Is<Offer>(_ => _.State == OfferState.Created)), Times.Never);
            mockOfferStore.Verify(_ => _.Update(It.Is<Offer>(_ => _.State == OfferState.Presented)), Times.Never);
            mockOfferStore.Verify(_ => _.Update(It.Is<Offer>(_ => _.State == OfferState.Redeemed)), Times.Exactly(3));
            mockOfferStore.Verify(_ => _.Context.SaveChanges(), Times.Never);

            if (eventKind == EventKinds.Purchase)
            {
                Assert.Equal(defaultValue, offerCreated.GrossRevenue);
                Assert.Equal(defaultValue, offerPresented.GrossRevenue);
            }
            Assert.True(offerCreated.RedeemedAt.HasValue);
            Assert.True(offerPresented.RedeemedAt.HasValue);
            Assert.True(offerCreated.RedeemedPromotionId.HasValue);
            Assert.True(offerPresented.RedeemedPromotionId.HasValue);
        }

        [Fact]
        public async Task QueryOffers_Returns_Offers()
        {
            // Arrange
            var mockStorageContext = Utility.MockStorageContext();
            var mockOfferStore = new Mock<IOfferStore>();
            var mockPromotionStore = new Mock<IRecommendableItemStore>();
            var mockPromotionsRecommendationStore = new Mock<IItemsRecommendationStore>();
            var mockPaginate = new Mock<IPaginate>();
            var customer = new Customer("customer-id", "John Smith");
            var source = new IntegratedSystem("test-source", "Test Source", IntegratedSystemTypes.Segment);
            var defaultValue = 10;
            var properties = new Dictionary<string, object>()
            {
                { "promotionId", 1 },
                { "value", defaultValue }
            };
            var promotion = new RecommendableItem("test-promotion", "Test Promotion", 0, 1, BenefitType.Fixed, 1, PromotionType.Other, null)
            {
                Id = 1
            };
            var customerEvent = new CustomerEvent(
                customer, "test-event", DateTimeOffset.Now, source,
                EventKinds.Purchase, "test-event", properties, 1);
            var recommender = new ItemsRecommender(
                "test-recommender", "Test Recommender", RecommendableItem.DefaultRecommendableItem,
                new RecommendableItem[] { promotion }, null, null, null);
            var recommendingContext = new RecommendingContext(
                new RecommendationCorrelator(recommender), null, null
            );
            var recommendation = new ItemsRecommendation(
                recommender,
                recommendingContext,
                new ScoredRecommendableItem[] { new ScoredRecommendableItem(promotion, 0.25, null) });
            var offer = new Offer(recommendation);

            mockOfferStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockOfferStore
                .Setup(_ => _.Query(It.IsAny<EntityStoreQueryOptions<Offer>>()))
                .ReturnsAsync(
                    new Paginated<Offer>(new List<Offer>
                    {
                        offer
                    }, 1, 1, 1
                ));
            // Act
            var workflow = new OfferWorkflows(
                Utility.MockLogger<OfferWorkflows>().Object,
                mockOfferStore.Object,
                mockPromotionStore.Object,
                mockPromotionsRecommendationStore.Object,
                Utility.DateTimeProvider()
            );
            var results = await workflow.QueryOffers(recommender, mockPaginate.Object, state: null);
            // Assert
            mockOfferStore.Verify(_ => _.Query(It.IsAny<EntityStoreQueryOptions<Offer>>()), Times.Once);
            Assert.NotEmpty(results.Items);
        }
    }
}