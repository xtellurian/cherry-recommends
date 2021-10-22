using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public class RandomProductRecommender : IRecommenderModelClient<ProductRecommenderModelOutputV1>
    {
        private readonly IProductStore productStore;

        public RandomProductRecommender(IProductStore productStore)
        {
            this.productStore = productStore;
        }
        public async Task<ProductRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext recommendingContext, IModelInput input)
        {
            var random = new Random();
            var productRecommender = (ProductRecommender)recommender;
            var products = new List<Product>();
            if (productRecommender.Products == null || !productRecommender.Products.Any())
            {
                products.AddRange((await productStore.Query(1)).Items);
            }
            else
            {

                products.AddRange(productRecommender.Products);
            }
            var index = random.Next(products.Count);
            var product = products[index];
            return new ProductRecommenderModelOutputV1
            {
                ProductId = product.Id,
                ProductCommonId = product.CommonId,
                Product = product
            };
        }

        public Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action)
        {
            context.Logger.LogWarning($"{this.GetType()} cannot be rewarded");
            return Task.CompletedTask;
        }
    }
}