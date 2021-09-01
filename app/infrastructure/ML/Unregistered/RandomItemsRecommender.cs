using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public class RandomItemsRecommender : IRecommenderModelClient<IModelInput, ItemsRecommenderModelOutputV1>
    {
        private readonly IRecommendableItemStore itemStore;

        public RandomItemsRecommender(IRecommendableItemStore itemStore)
        {
            this.itemStore = itemStore;
        }
        public async Task<ItemsRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext recommendingContext, IModelInput input)
        {
            var random = new Random();
            var itemsRecommender = (ItemsRecommender)recommender;
            var items = new List<RecommendableItem>();
            if (itemsRecommender.Items == null || !itemsRecommender.Items.Any())
            {
                items.AddRange((await itemStore.Query(1)).Items);
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
                ScoredItems = itemsToRecommend.Select(_ => new ScoredRecommendableItem(_, random.Next())).ToList()
            };
        }

        public Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action)
        {
            context.Logger.LogWarning($"{this.GetType()} cannot be rewarded");
            return Task.CompletedTask;
        }
    }
}