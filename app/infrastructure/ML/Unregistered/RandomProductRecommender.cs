using System;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class RandomProductRecommender : IModelClient<ProductRecommenderModelInputV1, ProductRecommenderModelOutputV1>
    {
        private readonly IProductStore productStore;

        public RandomProductRecommender(IProductStore productStore)
        {
            this.productStore = productStore;
        }
        public async Task<ProductRecommenderModelOutputV1> Invoke(ModelRegistration model, string version, ProductRecommenderModelInputV1 input)
        {
            // model should be null
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