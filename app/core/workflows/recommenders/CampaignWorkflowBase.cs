using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
    public abstract class CampaignWorkflowBase<TCampaign> : IWorkflow where TCampaign : CampaignEntityBase
    {
        protected readonly ICampaignStore<TCampaign> store;
        protected readonly IIntegratedSystemStore systemStore;
        protected readonly IMetricStore featureStore;
        protected readonly ISegmentStore segmentStore;
        private readonly IChannelStore channelStore;
        private readonly IRecommendableItemStore promotionStore;
        private readonly IArgumentRuleStore argumentRuleStore;
        private readonly IRecommenderReportImageWorkflow imageWorkflows;

        protected CampaignWorkflowBase(ICampaignStore<TCampaign> store,
                                          IStoreCollection storeCollection,
                                          IRecommenderReportImageWorkflow imageWorkflows)
        {
            this.store = store;
            this.systemStore = storeCollection.ResolveStore<IIntegratedSystemStore, IntegratedSystem>();
            this.featureStore = storeCollection.ResolveStore<IMetricStore, Metric>(); ;
            this.segmentStore = storeCollection.ResolveStore<ISegmentStore, Segment>(); ;
            this.channelStore = storeCollection.ResolveStore<IChannelStore, ChannelBase>();
            this.promotionStore = storeCollection.ResolveStore<IRecommendableItemStore, RecommendableItem>();
            this.argumentRuleStore = storeCollection.ResolveStore<IArgumentRuleStore, ArgumentRule>();
            this.imageWorkflows = imageWorkflows;
        }

        // ------ ADD/REMOVE DESTINATIONS -----
        public async Task<CampaignEntityBase> RemoveDestination(TCampaign recommender, long destinationId)
        {
            await store.LoadMany(recommender, _ => _.RecommendationDestinations);
            var destination = recommender.RecommendationDestinations.FirstOrDefault(_ => _.Id == destinationId);
            if (destination == null)
            {
                throw new BadRequestException($"Destination Id {destinationId} is not a destination of Campaign Id {recommender.Id}");
            }

            recommender.RecommendationDestinations.Remove(destination);
            await store.Context.SaveChanges();
            return recommender;
        }

        public async Task<RecommendationDestinationBase> AddDestination(TCampaign recommender,
                                                                        long systemId,
                                                                        string destinationType,
                                                                        string endpoint)
        {
            var maxDestinations = 5;
            var system = await systemStore.Read(systemId);
            await store.LoadMany(recommender, _ => _.RecommendationDestinations);

            if (recommender.RecommendationDestinations.Count > maxDestinations)
            {
                throw new BadRequestException($"The maximum number of destinations is {maxDestinations}");
            }

            RecommendationDestinationBase destination;
            switch (destinationType)
            {
                case null:
                    throw new BadRequestException("DestinationType cannot be null");
                case RecommendationDestinationBase.WebhookDestinationType:
                    destination = new WebhookDestination(recommender, system, endpoint);
                    break;
                case RecommendationDestinationBase.SegmentSourceFunctionDestinationType:
                    destination = new SegmentSourceFunctionDestination(recommender, system, endpoint);
                    break;
                default:
                    throw new BadRequestException($"DestinationType {destinationType} is an unknown type");

            }


            recommender.RecommendationDestinations.Add(destination);
            await store.Context.SaveChanges();

            if (destination.Discriminator == "RecommendationDestinationBase")
            {
                recommender.RecommendationDestinations.Remove(destination);
                await store.Context.SaveChanges();

                throw new ConfigurationException($"Could not create destination of type {destination.GetType().Name}. You may need a database migration");
            }

            return destination;
        }

        // ------ SET LEARNING FEATURES -----

        public async Task<TCampaign> SetLearningMetrics(TCampaign recommender, IEnumerable<string> metricIds, bool? useInternalId = null)
        {
            var metrics = new List<Metric>();
            foreach (var metricId in metricIds)
            {
                metrics.Add(await featureStore.GetEntity(metricId, useInternalId));
            }

            await store.LoadMany(recommender, _ => _.LearningFeatures);

            recommender.LearningFeatures = metrics;

            await store.Context.SaveChanges();
            return recommender;
        }

        public async Task<byte[]> DownloadReportImage(CampaignEntityBase recommender)
        {
            return await imageWorkflows.DownloadImage(recommender);
        }

        // ------ ADD/REMOVE CHANNELS -----
        public async Task<ChannelBase> AddChannel(TCampaign recommender, long channelId)
        {
            var maxChannels = 2;
            await store.LoadMany(recommender, _ => _.Channels);

            if (recommender.Channels.Count >= maxChannels)
            {
                throw new BadRequestException($"The maximum number of channels is {maxChannels}");
            }

            ChannelBase channel = await channelStore.Read(channelId);
            if (channel == null)
            {
                throw new BadRequestException($"Channel Id {channelId} is not a valid channel");
            }

            if (recommender.Channels.Any(_ => _.Id == channelId))
            {
                throw new BadRequestException($"Channel Id {channelId} is already a channel of recommender Id {recommender.Id}");
            }

            recommender.Channels.Add(channel);
            await store.Context.SaveChanges();
            return channel;
        }

        public async Task<CampaignEntityBase> RemoveChannel(TCampaign recommender, long channelId)
        {
            await store.LoadMany(recommender, _ => _.Channels);
            var channel = recommender.Channels.FirstOrDefault(_ => _.Id == channelId);
            if (channel == null)
            {
                throw new BadRequestException($"Channel Id {channelId} is not a channel of Campaign Id {recommender.Id}");
            }

            recommender.Channels.Remove(channel);
            await store.Context.SaveChanges();
            return recommender;
        }

        // ArgumentRule rules
        public async Task<ChoosePromotionArgumentRule> CreateChoosePromotionArgumentRule(TCampaign campaign, long argumentId, long promotionId, string argumentValue)
        {
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            await store.LoadMany(campaign, _ => _.Arguments);
            var arg = campaign.Arguments.First(_ => _.Id == argumentId);
            var promotion = await promotionStore.Read(promotionId);

            var rule = new ChoosePromotionArgumentRule(campaign, arg, promotion, argumentValue);
            rule.Validate();
            campaign.ArgumentRules.Add(rule);
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return rule;
        }
        public async Task<ChoosePromotionArgumentRule> UpdateChoosePromotionArgumentRule(TCampaign campaign, long ruleId, long promotionId, string argumentValue)
        {
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            await store.LoadMany(campaign, _ => _.Arguments);

            if (!(campaign.ArgumentRules.FirstOrDefault(_ => _.Id == ruleId) is ChoosePromotionArgumentRule rule))
            {
                throw new EntityNotFoundException(typeof(ChoosePromotionArgumentRule), ruleId, "No rule found");
            }

            var promotion = await promotionStore.Read(promotionId);
            rule.PromotionId = promotionId;
            rule.Promotion = promotion;
            rule.ArgumentValue = argumentValue;
            rule.Validate();
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return rule;
        }

        public async Task<ChooseSegmentArgumentRule> CreateChooseSegmentArgumentRule(TCampaign campaign, long argumentId, long segmentId, string argumentValue)
        {
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            await store.LoadMany(campaign, _ => _.Arguments);
            var arg = campaign.Arguments.First(_ => _.Id == argumentId);
            var segment = await segmentStore.Read(segmentId);

            var rule = new ChooseSegmentArgumentRule(campaign, arg, segment, argumentValue);
            rule.Validate();
            campaign.ArgumentRules.Add(rule);
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return rule;
        }

        public async Task<ChooseSegmentArgumentRule> UpdateChooseSegmentArgumentRule(TCampaign campaign, long ruleId, long segmentId, string argumentValue)
        {
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            await store.LoadMany(campaign, _ => _.Arguments);

            if (!(campaign.ArgumentRules.FirstOrDefault(_ => _.Id == ruleId) is ChooseSegmentArgumentRule rule))
            {
                throw new EntityNotFoundException(typeof(ChooseSegmentArgumentRule), ruleId, "No rule found");
            }

            var segment = await segmentStore.Read(segmentId);
            rule.SegmentId = segmentId;
            rule.Segment = segment;
            rule.ArgumentValue = argumentValue;
            rule.Validate();
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return rule;
        }

        public async Task DeleteArgumentRule(TCampaign campaign, long ruleId)
        {
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            var rule = campaign.ArgumentRules.First(_ => _.Id == ruleId);

            await argumentRuleStore.Remove(ruleId);
            campaign.ArgumentRules.Remove(rule);

            await store.Update(campaign);
            await argumentRuleStore.Context.SaveChanges();
            await store.Context.SaveChanges();
        }
    }
}