using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class ProductWorkflows : IWorkflow
    {
        private readonly IProductStore store;
        private readonly IStorageContext storageContext;

        public ProductWorkflows(IProductStore store, IStorageContext storageContext)
        {
            this.store = store;
            this.storageContext = storageContext;
        }

        public async Task<Product> CreateProduct(string commonId, string name, double? listPrice, double? directCost, string? description, DynamicPropertyDictionary? properties)
        {
            var product = await store.Create(new Product(commonId, name, listPrice, directCost, properties)
            {
                Description = description,
            });

            await storageContext.SaveChanges();
            return product;
        }
    }
}