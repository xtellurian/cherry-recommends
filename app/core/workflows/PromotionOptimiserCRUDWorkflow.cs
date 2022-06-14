using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class PromotionOptimiserCRUDWorkflow : IWorkflow, IPromotionOptimiserCRUDWorkflow
    {
        private readonly IPromotionOptimiserStore store;
        private readonly IPromotionsCampaignStore campaignStore;
        private readonly ISegmentStore segmentStore;

        public PromotionOptimiserCRUDWorkflow(IStoreCollection storeCollection)
        {
            this.store = storeCollection.ResolveStore<IPromotionOptimiserStore, PromotionOptimiser>();
            this.campaignStore = storeCollection.ResolveStore<IPromotionsCampaignStore, PromotionsCampaign>();
            this.segmentStore = storeCollection.ResolveStore<ISegmentStore, Segment>();
        }


        public async Task<PromotionOptimiser> Create(PromotionsCampaign campaign)
        {
            if (campaign.BaselineItemId is null)
            {
                throw new BadRequestException("Recommender has null baseline");
            }
            await campaignStore.LoadMany(campaign, _ => _.Items); // ensure the items are loaded.
            var optimiser = await store.Create(new PromotionOptimiser(campaign));
            campaign.Optimiser = optimiser;
            optimiser.InitialiseWeights(campaign);
            await store.Context.SaveChanges();
            return optimiser;
        }

        /// <summary>
        /// Read the Optimiser for a campaign from the database.
        /// This will automatically update any weights as necessary, if the items have changed from the campaign.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="useInternalId"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<PromotionOptimiser> Read(string campaignId, bool? useInternalId = null)
        {
            var campaign = await campaignStore.GetEntity(campaignId, useInternalId);
            await campaignStore.Load(campaign, _ => _.Optimiser);

            if (campaign.Optimiser == null)
            {
                if (campaign.UseOptimiser)
                {
                    return await Create(campaign);
                }
                else
                {
                    throw new BadRequestException("Cannot auto-create optimiser when Campaign has UseOptimiser=false");
                }
            }

            if (campaign.UseOptimiser)
            {
                // update the default distribution if using optimiser
                campaign.Optimiser.UpdateWeights(campaign);
                await campaignStore.Context.SaveChanges();
            }

            return campaign.Optimiser;
        }

        public async Task<PromotionOptimiser> UpdateWeight(string campaignId, long weightId, double weight, long? segmentId = null, bool? useInternalId = null)
        {
            if (weight < 0)
            {
                throw new BadRequestException("Weight must not be negative");
            }
            var optimiser = await Read(campaignId, useInternalId);
            var optimiserWeights = optimiser.Weights.Where(_ => _.SegmentId == segmentId).ToList();
            if (optimiserWeights.Any(_ => _.Id == weightId))
            {
                var w = optimiserWeights.First(_ => _.Id == weightId);
                w.Weight = weight;
            }
            else
            {
                throw new BadRequestException($"Weight with ID {weightId} doesn't exist on Optimiser {optimiser.Id}");
            }

            optimiserWeights.Normalize();
            await store.Context.SaveChanges();
            return optimiser;
        }

        public async Task<bool> Delete(long id)
        {
            var success = await store.Remove(id);
            await store.Context.SaveChanges();
            return success;
        }

        public async Task<PromotionOptimiser> UpdateAllWeights(string campaignId, IEnumerable<IWeighted> weights, long? segmentId = null, bool? useInternalId = null)
        {
            var optimiser = await Read(campaignId, useInternalId);
            optimiser.UpdateWeights(optimiser.Recommender);

            var optimiserWeights = optimiser.Weights.Where(_ => _.SegmentId == segmentId).ToList();
            foreach (var w in weights)
            {
                if (optimiserWeights.Any(_ => _.Id == w.Id))
                {
                    optimiserWeights.First(_ => _.Id == w.Id).Weight = w.Weight;
                }
                else
                {
                    throw new BadRequestException($"Optimiser {optimiser.Id} does not contain weight id {w.Id}");
                }
            }

            optimiserWeights.Normalize();
            await store.Context.SaveChanges();
            return optimiser;
        }

        public async Task<IEnumerable<PromotionOptimiserWeight>> ReadWeights(string campaignId, long? segmentId, bool? useInternalId = null)
        {
            var campaign = await campaignStore.GetEntity(campaignId, useInternalId);
            await campaignStore.Load(campaign, _ => _.Optimiser);
            if (campaign.Optimiser == null)
            {
                throw new BadRequestException("Campaign Optimiser is null");
            }

            var optimiser = campaign.Optimiser;
            if (campaign.UseOptimiser && optimiser.Weights.Any(_ => _.SegmentId == segmentId))
            {
                // only update if using optimiser
                optimiser.UpdateWeights(campaign);
                await campaignStore.Context.SaveChanges();
            }

            var weights = optimiser.Weights.Where(_ => _.SegmentId == segmentId).ToList();
            return weights;
        }

        public async Task<PromotionOptimiser> AddSegment(string campaignId, long segmentId, bool? useInternalId = null)
        {
            var campaign = await campaignStore.GetEntity(campaignId, useInternalId);
            await campaignStore.Load(campaign, _ => _.Optimiser);
            if (campaign.Optimiser == null)
            {
                throw new BadRequestException("Campaign Optimiser is null");
            }

            var optimiser = campaign.Optimiser;
            Segment segment = await segmentStore.Read(segmentId);
            if (segment == null)
            {
                throw new BadRequestException($"Segment Id {segmentId} is not a valid segment");
            }

            if (optimiser.Weights.Any(_ => _.SegmentId == segmentId))
            {
                throw new BadRequestException($"Segment Id {segmentId} is already a segment of campaign Id {campaign.Id}");
            }

            if (campaign.UseOptimiser)
            {
                // only update if using optimiser
                optimiser.InitialiseWeights(campaign, segmentId);
            }

            await store.Context.SaveChanges();
            return optimiser;
        }

        public async Task<Paginated<Segment>> ReadSegments(string campaignId, bool? useInternalId = null)
        {
            var campaign = await campaignStore.GetEntity(campaignId, useInternalId);
            await campaignStore.Load(campaign, _ => _.Optimiser);
            if (campaign.Optimiser == null)
            {
                throw new BadRequestException("Campaign Optimiser is null");
            }

            var optimiser = campaign.Optimiser;

            // load segments
            var segmentIds = optimiser.Weights
                .Where(_ => _.SegmentId != null)
                .Select(_ => _.SegmentId).Distinct().ToList();

            var segments = await segmentStore.Query(new EntityStoreQueryOptions<Segment>(1, _ => segmentIds.Contains(_.Id)));
            return segments;
        }

        public async Task<bool> RemoveSegment(string campaignId, long segmentId, bool? useInternalId = null)
        {
            var campaign = await campaignStore.GetEntity(campaignId, useInternalId);
            await campaignStore.Load(campaign, _ => _.Optimiser);
            if (campaign.Optimiser == null)
            {
                throw new BadRequestException("Campaign Optimiser is null");
            }

            var optimiser = campaign.Optimiser;

            if (campaign.UseOptimiser && optimiser.Weights.Any(_ => _.SegmentId == segmentId))
            {
                // remove weights associated with the segment id
                optimiser.RemoveWeights(campaign, segmentId);
            }

            await store.Context.SaveChanges();
            return true;
        }
    }
}
