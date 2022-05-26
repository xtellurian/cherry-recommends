using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Workflows
{
#nullable enable
    public class ItemRecommenderInvokationWorkflowTests
    {

        private class MockDependencies
        {
            public Mock<ILogger<ItemsRecommenderInvokationWorkflows>> MockLogger { get; set; } = Utility.MockLogger<ItemsRecommenderInvokationWorkflows>();
            public Mock<IStorageContext> MockContext { get; set; } = new Mock<IStorageContext>();
            public IDateTimeProvider DateTimeProvider { get; set; } = new SystemDateTimeProvider();
            public Mock<IRecommendationCache<ItemsRecommender, ItemsRecommendation>> MockRecommendationCache { get; set; } = new Mock<IRecommendationCache<ItemsRecommender, ItemsRecommendation>>();
            public Mock<IRecommenderModelClientFactory> MockRecommenderModelClientFactory { get; set; } = new Mock<IRecommenderModelClientFactory>();
            public Mock<ICustomerWorkflow> MockCustomerWorkflow { get; set; } = new Mock<ICustomerWorkflow>();
            public Mock<IBusinessWorkflow> MockBusinessWorkflow { get; set; } = new Mock<IBusinessWorkflow>();
            public Mock<IHistoricCustomerMetricStore> MockHistoricCustomerMetricStore { get; set; } = new Mock<IHistoricCustomerMetricStore>();
            public Mock<IRecommendableItemStore> MockRecommendableItemStore { get; set; } = new Mock<IRecommendableItemStore>();
            public Mock<IWebhookSenderClient> MockWebhookSenderClient { get; set; } = new Mock<IWebhookSenderClient>();
            public Mock<IRecommendationCorrelatorStore> MockCorrelatorStore =>
                new Mock<IRecommendationCorrelatorStore>()
                .WithContext<Mock<IRecommendationCorrelatorStore>, IRecommendationCorrelatorStore, RecommendationCorrelator>(MockContext.Object);
            public Mock<IItemsRecommenderStore> MockItemsRecommenderStore =>
                new Mock<IItemsRecommenderStore>()
                .WithContext<Mock<IItemsRecommenderStore>, IItemsRecommenderStore, ItemsRecommender>(MockContext.Object);
            public Mock<IItemsRecommendationStore> MockItemsRecommendationStore { get; set; } = new Mock<IItemsRecommendationStore>();
            public Mock<IAudienceStore> MockAudienceStore { get; set; } = new Mock<IAudienceStore>();
            public Mock<IInternalOptimiserClientFactory> MockInternalOptimiserClientFactory { get; set; } = new Mock<IInternalOptimiserClientFactory>();
            public Mock<IDiscountCodeWorkflow> MockDiscountCodeWorkflow { get; set; } = new Mock<IDiscountCodeWorkflow>();
            public Mock<IKlaviyoSystemWorkflow> MockKlaviyoWorkflow { get; set; } = new Mock<IKlaviyoSystemWorkflow>();
            public Mock<IOfferStore> MockOfferStore { get; set; } = new Mock<IOfferStore>();
            public Mock<IArgumentRuleStore> MockArgumentRuleStore { get; set; } = new Mock<IArgumentRuleStore>();
            public MockStoreCollection MockStoreCollection => new MockStoreCollection()
               .With<IHistoricCustomerMetricStore, HistoricCustomerMetric>(MockHistoricCustomerMetricStore)
               .With<IRecommendableItemStore, RecommendableItem>(MockRecommendableItemStore)
               .With<IRecommendationCorrelatorStore, RecommendationCorrelator>(MockCorrelatorStore)
               .With<IItemsRecommenderStore, ItemsRecommender>(MockItemsRecommenderStore)
               .With<IItemsRecommendationStore, ItemsRecommendation>(MockItemsRecommendationStore)
               .With<IAudienceStore, Audience>(MockAudienceStore)
               .With<IOfferStore, Offer>(MockOfferStore)
               .With<IArgumentRuleStore, ArgumentRule>(MockArgumentRuleStore);
        }

        [Theory]
        [InlineData(null, "business", PromotionRecommenderTargetTypes.Customer)]
        [InlineData("customer", null, PromotionRecommenderTargetTypes.Business)]
        public async Task InvokeForWrongTarget_Throws(string? customerId, string? businessId, PromotionRecommenderTargetTypes targetType)
        {

            var deps = new MockDependencies();
            var sut = new ItemsRecommenderInvokationWorkflows(
                deps.MockLogger.Object,
                deps.DateTimeProvider,
                deps.MockRecommendationCache.Object,
                deps.MockRecommenderModelClientFactory.Object,
                deps.MockCustomerWorkflow.Object,
                deps.MockBusinessWorkflow.Object,
                deps.MockStoreCollection,
                deps.MockItemsRecommenderStore.Object,
                deps.MockWebhookSenderClient.Object,
                deps.MockInternalOptimiserClientFactory.Object,
                deps.MockDiscountCodeWorkflow.Object,
                deps.MockKlaviyoWorkflow.Object
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

        [Fact]
        public async Task DisabledRecommender_OnInvoked_Throws()
        {
            var deps = new MockDependencies();
            var sut = new ItemsRecommenderInvokationWorkflows(
                deps.MockLogger.Object,
                deps.DateTimeProvider,
                deps.MockRecommendationCache.Object,
                deps.MockRecommenderModelClientFactory.Object,
                deps.MockCustomerWorkflow.Object,
                deps.MockBusinessWorkflow.Object,
                deps.MockStoreCollection,
                deps.MockItemsRecommenderStore.Object,
                deps.MockWebhookSenderClient.Object,
                deps.MockInternalOptimiserClientFactory.Object,
                deps.MockDiscountCodeWorkflow.Object,
                deps.MockKlaviyoWorkflow.Object
            );

            var baseline = new RecommendableItem("item1", "Item 1", 1, 1, BenefitType.Percent, 1, PromotionType.Discount, null);
            var other = new RecommendableItem("item2", "Item 2", 1, 1, BenefitType.Percent, 1, PromotionType.Discount, null);
            var allItems = new List<RecommendableItem> { baseline, other };
            var recommender = new ItemsRecommender("recommenderId", "Recomender", baseline, allItems, null, null, null)
            {
                TargetType = PromotionRecommenderTargetTypes.Customer,
                Settings = new RecommenderSettings
                {
                    Enabled = false
                }
            };
            var input = new ItemsModelInputDto
            {
                CustomerId = "1234",
            };
            // act
            await Assert.ThrowsAsync<RecommenderInvokationException>(() => sut.InvokeItemsRecommender(recommender, input));
        }

        [Fact]
        public async Task ArgumentPromotionRule_ReturnsPromotion()
        {
            var deps = new MockDependencies();
            var sut = new ItemsRecommenderInvokationWorkflows(
                deps.MockLogger.Object,
                deps.DateTimeProvider,
                deps.MockRecommendationCache.Object,
                deps.MockRecommenderModelClientFactory.Object,
                deps.MockCustomerWorkflow.Object,
                deps.MockBusinessWorkflow.Object,
                deps.MockStoreCollection,
                deps.MockItemsRecommenderStore.Object,
                deps.MockWebhookSenderClient.Object,
                deps.MockInternalOptimiserClientFactory.Object,
                deps.MockDiscountCodeWorkflow.Object,
                deps.MockKlaviyoWorkflow.Object
            );
            // setup dependency calls
            deps.MockItemsRecommendationStore.SetupStoreCreate<Mock<IItemsRecommendationStore>, IItemsRecommendationStore, ItemsRecommendation>();
            deps.MockOfferStore.SetupStoreCreate<Mock<IOfferStore>, IOfferStore, Offer>();
            deps.MockAudienceStore.SetupStoreCreate<Mock<IAudienceStore>, IAudienceStore, Audience>();
            deps.MockAudienceStore.Setup(_ => _.GetAudience(It.IsAny<RecommenderEntityBase>())).ReturnsAsync(new EntityResult<Audience>(null));
            deps.MockCustomerWorkflow
                .Setup(_ => _.CreateOrUpdate(It.IsAny<PendingCustomer>(), It.IsAny<bool>()))
                .ReturnsAsync((PendingCustomer p, bool b) => p.ToCoreRepresentation());

            var baseline = new RecommendableItem("item1", "Item 1", 1, 1, BenefitType.Percent, 1, PromotionType.Discount, null);
            var toRecommend = new RecommendableItem("item2", "Item 2", 1, 1, BenefitType.Percent, 1, PromotionType.Discount, null);
            var other1 = new RecommendableItem(Utility.RandomString(), Utility.RandomString(), 1, 1, BenefitType.Fixed, 2, PromotionType.Other, null);
            var other2 = new RecommendableItem(Utility.RandomString(), Utility.RandomString(), 1, 1, BenefitType.Fixed, 3, PromotionType.Gift, null);
            var allItems = new List<RecommendableItem> { baseline, toRecommend, other1, other2 };
            var argCommonId = Utility.RandomString();
            var argValue = Utility.RandomString();
            var argument = new CampaignArgument(argCommonId, ArgumentTypes.Categorical, false);
            var campaign = new ItemsRecommender("recommenderId", "Recomender", baseline, allItems, null, null, null)
            {
                TargetType = PromotionRecommenderTargetTypes.Customer,
                Settings = new RecommenderSettings
                {
                    ThrowOnBadInput = true
                },
                Arguments = new List<CampaignArgument> { argument },
                ArgumentRules = new List<ArgumentRule>()
            };
            var rule = new ChoosePromotionArgumentRule(campaign, argument, toRecommend, argValue);
            campaign.ArgumentRules.Add(rule);
            var input = new ItemsModelInputDto
            {
                CustomerId = "1234",
                Arguments = new Dictionary<string, object>
                {
                    { argCommonId, argValue }
                }
            };
            // act
            var recommendation = await sut.InvokeItemsRecommender(campaign, input);

            Assert.Equal(toRecommend, recommendation.ScoredItems.First().Item);
        }
    }
}