using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.ML
{
    public class PromotionsOptimiserClient : IRecommenderModelClient<ItemsRecommenderModelOutputV1>
    {
        private readonly IPromotionsCampaignStore itemsRecommenderStore;

        public PromotionsOptimiserClient(IPromotionsCampaignStore itemsRecommenderStore)
        {
            this.itemsRecommenderStore = itemsRecommenderStore;
        }
        public async Task<ItemsRecommenderModelOutputV1> Invoke(ICampaign recommender, RecommendingContext context, IModelInput input)
        {
            // var randomSelector = new WeightedRandomSelector<PromotionOptimiserWeight>();
            if (recommender is PromotionsCampaign itemsRecommender)
            {
                await itemsRecommenderStore.Load(itemsRecommender, _ => _.Optimiser);
                if (!itemsRecommender.UseOptimiser)
                {
                    throw new BadRequestException("Campaign has UseOptimiser set to False");
                }
                if (itemsRecommender.Optimiser == null)
                {
                    throw new BadRequestException("Campaign Optimiser is null");
                }

                // construct all the items 
                var optimiser = itemsRecommender.Optimiser;

                // get the first segment the customer belongs to, null for the default
                long? customerSegmentId = null;
                var segmentIds = context.Customer.Segments?.Select(_ => _.SegmentId);
                if (segmentIds != null && segmentIds.Any())
                {
                    customerSegmentId = segmentIds.First();
                    // if optimiser doesn't have distribution for the segment id, use the default
                    if (!optimiser.Weights.Any(_ => _.SegmentId == customerSegmentId))
                    {
                        customerSegmentId = null;
                    }
                }

                optimiser = optimiser.UpdateWeights(itemsRecommender);
                var optimiserWeights = optimiser.Weights.Where(_ => _.SegmentId == customerSegmentId);

                var selector = new WeightedRandomSelector<PromotionOptimiserWeight>(optimiserWeights);
                var output = new ItemsRecommenderModelOutputV1();
                var breakoutCount = 0;
                while (output.ScoredItems.Count < (itemsRecommender.NumberOfItemsToRecommend ?? 1))
                {
                    breakoutCount++;
                    var selectedPromotion = selector.Choose();
                    if (!output.ScoredItems.Any(_ => _.ItemId == selectedPromotion.PromotionId))
                    {
                        output.ScoredItems.Add(new ScoredRecommendableItem(selectedPromotion.Promotion, selectedPromotion.Weight));
                    }
                    if (breakoutCount > 1000)
                    {
                        throw new ModelInvokationException("Optimiser ran too many times");
                    }
                }

                return output;
            }
            else
            {
                throw new System.NotImplementedException("Promotions recommenders do not support internal optimiser clients.");
            }
        }
    }
}