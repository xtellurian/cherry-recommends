using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure
{
    public class RandomItemsRecommender : IRecommenderModelClient<ItemsRecommenderModelOutputV1>
    {
        private readonly IRecommendableItemStore itemStore;

        public RandomItemsRecommender(IRecommendableItemStore itemStore)
        {
            this.itemStore = itemStore;
        }
        public async Task<ItemsRecommenderModelOutputV1> Invoke(ICampaign recommender, RecommendingContext recommendingContext, IModelInput input)
        {
            var random = new Random();
            var itemsRecommender = (PromotionsCampaign)recommender;
            var items = new List<RecommendableItem>();
            if (itemsRecommender.Items == null || !itemsRecommender.Items.Any())
            {
                items.AddRange((await itemStore.Query(new EntityStoreQueryOptions<RecommendableItem>())).Items);
            }
            else
            {
                items.AddRange(itemsRecommender.Items);
            }

            // select some random items
            items.Shuffle();
            var itemsToRecommend = items.Take(itemsRecommender.NumberOfItemsToRecommend ?? 1).ToList();

            return new ItemsRecommenderModelOutputV1
            {
                ScoredItems = itemsToRecommend.Select(_ => new ScoredRecommendableItem(_, Math.Round(random.NextDouble(), 4))).ToList()
            };
        }

        public Task Reward(ICampaign recommender, RewardingContext context)
        {
            context.Logger.LogWarning("{type} cannot be rewarded", this.GetType());
            return Task.CompletedTask;
        }
    }
}