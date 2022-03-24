using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.ML
{
    public class PromotionsOptimiserClient : IRecommenderModelClient<ItemsRecommenderModelOutputV1>
    {
        private readonly IItemsRecommenderStore itemsRecommenderStore;

        public PromotionsOptimiserClient(IItemsRecommenderStore itemsRecommenderStore)
        {
            this.itemsRecommenderStore = itemsRecommenderStore;
        }
        public async Task<ItemsRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext context, IModelInput input)
        {
            // var randomSelector = new WeightedRandomSelector<PromotionOptimiserWeight>();
            if (recommender is ItemsRecommender itemsRecommender)
            {
                await itemsRecommenderStore.Load(itemsRecommender, _ => _.Optimiser);
                if (!itemsRecommender.UseOptimiser)
                {
                    throw new BadRequestException("Recommender has UseOptimiser set to False");
                }
                if (itemsRecommender.Optimiser == null)
                {
                    throw new BadRequestException("Recommender Optimiser is null");
                }

                // construct all the items 
                var optimiser = itemsRecommender.Optimiser;
                optimiser = optimiser.UpdateWeights(itemsRecommender);

                var selector = new WeightedRandomSelector<PromotionOptimiserWeight>(optimiser.Weights);
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