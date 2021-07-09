using System;
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
        public async Task<ProductRecommenderModelOutputV1> Invoke(IRecommender recommender, string version, ProductRecommenderModelInputV1 input)
        {
            // model should be null
            var productRecommender = (ProductRecommender)recommender;
            var random = new Random();
            var products = await productStore.Query(1);
            var productList = products.Items.ToList();
            var index = random.Next(productList.Count - 1);
            var product = productList[index];
            return new ProductRecommenderModelOutputV1
            {
                ProductId = product.Id,
                ProductCommonId = product.CommonId,
                Product = product
            };
        }
    }
}