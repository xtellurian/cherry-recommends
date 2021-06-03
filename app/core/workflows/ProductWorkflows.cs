using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class ProductWorkflows : IWorkflow
    {
        private readonly IProductStore store;
        private readonly IStorageContext storageContext;

        public ProductWorkflows(IProductStore store, IStorageContext storageContext)
        {
            this.store = store;
            this.storageContext = storageContext;
        }

        public async Task<Product> CreateProduct(string name, string productId, string description = null)
        {
            var product = await store.Create(new Product(name, productId)
            {
                ProductId = productId,
                Description = description,
            });

            await storageContext.SaveChanges();
            return product;
        }

        public async Task<Product> UpdateProductSkusFromProductId(string productId, IEnumerable<SkuInput> skuInputs)
        {
            var result = (await store.Query(1, _ => _.ProductId == productId));
            var product = result.Items.First();

            product = AddOrUpdateSkus(product, skuInputs);
            await storageContext.SaveChanges();
            return product;
        }

        private Product AddOrUpdateSkus(Product product, IEnumerable<SkuInput> skuInputs)
        {
            var skuIds = skuInputs.Select(_ => _.SkuId);
            var existingSkus = product.Skus.Where(sku => skuIds.Contains(sku.SkuId));
            foreach (var e in existingSkus)
            {
                product.Skus.Remove(e);
            }

            var skus = skuInputs.Select(_ => new Sku(_.Name, _.SkuId, _.Price, _.Description));
            foreach (var s in skus)
            {

                product.Skus.Add(s);
            }
            return product;
        }

        public struct SkuInput
        {
            public SkuInput(string name, string skuId, string description, double price)
            {
                Name = name;
                SkuId = skuId;
                Description = description;
                Price = price;
            }

            public string Name { get; }
            public string SkuId { get; }
            public string Description { get; }
            public double Price { get; }
        }
    }
}