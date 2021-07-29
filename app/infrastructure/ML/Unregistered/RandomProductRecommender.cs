using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public class RandomProductRecommender : IRecommenderModelClient<ProductRecommenderModelInputV1, ProductRecommenderModelOutputV1>
    {
        private readonly IProductStore productStore;

        public RandomProductRecommender(IProductStore productStore)
        {
            this.productStore = productStore;
        }
        public async Task<ProductRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext recommendingContext, ProductRecommenderModelInputV1 input)
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
            var index = random.Next(products.Count - 1);
            var product = products[index];
            return new ProductRecommenderModelOutputV1
            {
                ProductId = product.Id,
                ProductCommonId = product.CommonId,
                Product = product
            };
        }
    }
}