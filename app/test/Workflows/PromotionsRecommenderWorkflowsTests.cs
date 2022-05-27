using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Campaigns;
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
            var mockPromoRecommenderStore = new Mock<IPromotionsCampaignStore>()
                .WithContext<Mock<IPromotionsCampaignStore>, IPromotionsCampaignStore, PromotionsCampaign>();
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
            var mockPromotionOptimiserCRUDWorkflow = new Mock<IPromotionOptimiserCRUDWorkflow>();
            var mockChannelStore = new Mock<IChannelStore>();

            var baselinePromo = new RecommendableItem("item1", "Item 1", null, 1, BenefitType.Percent, 0.2, PromotionType.Discount, null)
                .WithId();
            var promo2 = new RecommendableItem("item2", "Item 2", null, 1, BenefitType.Percent, 0.3, PromotionType.Discount, null)
                .WithId();
            var promo3 = new RecommendableItem("item3", "Item 3", null, 1, BenefitType.Percent, 0.4, PromotionType.Discount, null)
                .WithId();
            mockPromoStore.SetupCommonStoreRead<Mock<IRecommendableItemStore>, IRecommendableItemStore, RecommendableItem>(baselinePromo, promo2, promo3);

            mockPromoRecommenderStore.Setup(_ => _.Create(It.IsAny<PromotionsCampaign>())).ReturnsAsync((PromotionsCampaign r) => r);

            var mockStoreCollection = new MockStoreCollection()
               .With<IChannelStore, ChannelBase>(mockChannelStore)
               .With<IRecommendableItemStore, RecommendableItem>(mockPromoStore);

            var sut = new PromotionsCampaignWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object,
                mockChannelStore.Object,
                mockStoreCollection,
                mockPromotionOptimiserCRUDWorkflow.Object
            );

            var useOptimiser = false;

            // act
            var output = await sut.CreatePromotionsCampaign(
                new CreateCommonEntityModel("commonId", "Name"),
                baselineItemId: baselinePromo.CommonId,
                itemIds: new List<string> { promo2.CommonId, promo3.CommonId },
                segmentIds: null,
                channelIds: null,
                numberOfItemsToRecommend: 1,
                arguments: null,
                settings: new CampaignSettings(),
                useOptimiser: useOptimiser,
                targetMetricId: null,
                targetType: PromotionCampaignTargetTypes.Customer,
                useInternalId: null
            );

            // assert
            Assert.NotNull(output);
            Assert.Equal("commonId", output.CommonId);
            Assert.Equal(baselinePromo.Id, output.BaselineItemId);
            Assert.Equal(3, output.Items.Count); // 2 + baseline
            Assert.Contains(baselinePromo, output.Items);
            Assert.Equal(useOptimiser, output.UseOptimiser);

            mockPromotionOptimiserCRUDWorkflow.Verify(_ => _.Create(It.Is<PromotionsCampaign>(r => r.CommonId == output.CommonId)), useOptimiser ? Times.Once : Times.Never);
        }

        [Fact]
        public async Task AddChannelTest()
        {
            // arrange
            var mockPromoRecommenderStore = new Mock<IPromotionsCampaignStore>()
                .WithContext<Mock<IPromotionsCampaignStore>, IPromotionsCampaignStore, PromotionsCampaign>();
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
            var mockPromotionOptimiserCRUDWorkflow = new Mock<IPromotionOptimiserCRUDWorkflow>();
            var mockChannelStore = new Mock<IChannelStore>();

            var mockStoreCollection = new MockStoreCollection()
               .With<IChannelStore, ChannelBase>(mockChannelStore)
               .With<IRecommendableItemStore, RecommendableItem>(mockPromoStore);

            var sut = new PromotionsCampaignWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object,
                mockChannelStore.Object,
                mockStoreCollection,
                mockPromotionOptimiserCRUDWorkflow.Object
            );

            IntegratedSystem system = new IntegratedSystem("system1", "System1", IntegratedSystemTypes.Custom);
            WebhookChannel channel = new WebhookChannel("channel1", system)
            {
                Id = 1
            };

            PromotionsCampaign recommender = new PromotionsCampaign("Recommender1", "Recommender1", null, new List<RecommendableItem>(), null, null, null)
            {
                Id = 1
            };

            mockChannelStore.Setup(_ => _.Read(It.IsAny<long>())).ReturnsAsync(channel);
            var result = await sut.AddChannel(recommender, channel.Id);

            Assert.NotNull(result);
            Assert.Equal(channel.Id, result.Id);

            mockPromoRecommenderStore.Verify(_ => _.Context.SaveChanges(), Times.Once);
        }


        [Fact]
        public async Task AddChannel_Maximum_Test()
        {
            // arrange
            var mockPromoRecommenderStore = new Mock<IPromotionsCampaignStore>()
                .WithContext<Mock<IPromotionsCampaignStore>, IPromotionsCampaignStore, PromotionsCampaign>();
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
            var mockPromotionOptimiserCRUDWorkflow = new Mock<IPromotionOptimiserCRUDWorkflow>();
            var mockChannelStore = new Mock<IChannelStore>();
            var mockStoreCollection = new MockStoreCollection()
               .With<IChannelStore, ChannelBase>(mockChannelStore)
               .With<IRecommendableItemStore, RecommendableItem>(mockPromoStore);

            var sut = new PromotionsCampaignWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object,
                mockChannelStore.Object,
                mockStoreCollection,
                mockPromotionOptimiserCRUDWorkflow.Object
            );

            IntegratedSystem system = new IntegratedSystem("system1", "System1", IntegratedSystemTypes.Custom);
            WebhookChannel channel1 = new WebhookChannel("channel1", system)
            {
                Id = 1
            };

            WebhookChannel channel2 = new WebhookChannel("channel1", system)
            {
                Id = 2
            };

            PromotionsCampaign recommender = new PromotionsCampaign("Recommender1", "Recommender1", null, new List<RecommendableItem>(), null, null, null)
            {
                Id = 1
            };
            recommender.Channels.Add(channel1);
            recommender.Channels.Add(channel2);

            mockChannelStore.Setup(_ => _.Read(It.IsAny<long>())).ReturnsAsync(channel1);

            await Assert.ThrowsAsync<BadRequestException>(() => sut.AddChannel(recommender, channel1.Id));
        }

        [Fact]
        public async Task AddChannel_AlreadyExists_Test()
        {
            // arrange
            var mockPromoRecommenderStore = new Mock<IPromotionsCampaignStore>()
                .WithContext<Mock<IPromotionsCampaignStore>, IPromotionsCampaignStore, PromotionsCampaign>();
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
            var mockPromotionOptimiserCRUDWorkflow = new Mock<IPromotionOptimiserCRUDWorkflow>();
            var mockChannelStore = new Mock<IChannelStore>();
            var mockStoreCollection = new MockStoreCollection()
               .With<IChannelStore, ChannelBase>(mockChannelStore)
               .With<IIntegratedSystemStore, IntegratedSystem>(mockIntegratedSystemStore)
               .With<IRecommendableItemStore, RecommendableItem>(mockPromoStore);

            var sut = new PromotionsCampaignWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object,
                mockChannelStore.Object,
                mockStoreCollection,
                mockPromotionOptimiserCRUDWorkflow.Object
            );

            IntegratedSystem system = new IntegratedSystem("system1", "System1", IntegratedSystemTypes.Custom);
            WebhookChannel channel1 = new WebhookChannel("channel1", system)
            {
                Id = 1
            };

            PromotionsCampaign recommender = new PromotionsCampaign("Recommender1", "Recommender1", null, new List<RecommendableItem>(), null, null, null)
            {
                Id = 1
            };
            recommender.Channels.Add(channel1);
            mockChannelStore.Setup(_ => _.Read(It.IsAny<long>())).ReturnsAsync(channel1);

            await Assert.ThrowsAsync<BadRequestException>(() => sut.AddChannel(recommender, channel1.Id));
        }

        [Fact]
        public async Task RemoveChannelTest()
        {
            // arrange
            var mockPromoRecommenderStore = new Mock<IPromotionsCampaignStore>()
                .WithContext<Mock<IPromotionsCampaignStore>, IPromotionsCampaignStore, PromotionsCampaign>();
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
            var mockPromotionOptimiserCRUDWorkflow = new Mock<IPromotionOptimiserCRUDWorkflow>();
            var mockChannelStore = new Mock<IChannelStore>();
            var mockStoreCollection = new MockStoreCollection()
                .With<IChannelStore, ChannelBase>(mockChannelStore)
                .With<IRecommendableItemStore, RecommendableItem>(mockPromoStore);

            var sut = new PromotionsCampaignWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object,
                mockChannelStore.Object,
                mockStoreCollection,
                mockPromotionOptimiserCRUDWorkflow.Object
            );

            IntegratedSystem system = new IntegratedSystem("system1", "System1", IntegratedSystemTypes.Custom);
            WebhookChannel channel = new WebhookChannel("channel1", system)
            {
                Id = 1
            };
            PromotionsCampaign recommender = new PromotionsCampaign("Recommender1", "Recommender1", null, new List<RecommendableItem>(), null, null, null)
            {
                Id = 1
            };
            recommender.Channels.Add(channel);

            var result = await sut.RemoveChannel(recommender, channel.Id);

            Assert.NotNull(result);
            Assert.Empty(result.Channels);

            mockPromoRecommenderStore.Verify(_ => _.Context.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task RemoveChannel_NotExistInRecommender_Test()
        {
            // arrange
            var mockPromoRecommenderStore = new Mock<IPromotionsCampaignStore>()
                .WithContext<Mock<IPromotionsCampaignStore>, IPromotionsCampaignStore, PromotionsCampaign>();
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
            var mockPromotionOptimiserCRUDWorkflow = new Mock<IPromotionOptimiserCRUDWorkflow>();
            var mockChannelStore = new Mock<IChannelStore>();

            var mockStoreCollection = new MockStoreCollection()
               .With<IChannelStore, ChannelBase>(mockChannelStore)
               .With<IRecommendableItemStore, RecommendableItem>(mockPromoStore);

            var sut = new PromotionsCampaignWorkflows(
                mockPromoRecommenderStore.Object,
                mockPromoRecommendationStore.Object,
                mockMetricStore.Object,
                mockCategoricalOptimiserClient.Object,
                mockModelRegistrationStore.Object,
                mockAudienceStore.Object,
                mockReportWorkflow.Object,
                mockPromoStore.Object,
                mockChannelStore.Object,
                mockStoreCollection,
                mockPromotionOptimiserCRUDWorkflow.Object
            );

            IntegratedSystem system = new IntegratedSystem("system1", "System1", IntegratedSystemTypes.Custom);
            WebhookChannel channel = new WebhookChannel("channel1", system)
            {
                Id = 1
            };
            PromotionsCampaign recommender = new PromotionsCampaign("Recommender1", "Recommender1", null, new List<RecommendableItem>(), null, null, null)
            {
                Id = 1
            };

            await Assert.ThrowsAsync<BadRequestException>(() => sut.RemoveChannel(recommender, channel.Id));
        }
    }
}