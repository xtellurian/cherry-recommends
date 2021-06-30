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

        public async Task<Product> CreateProduct(string commonId, string name, double? listPrice, string description = null, double? directCost = null)
        {
            var product = await store.Create(new Product(commonId, name, listPrice ?? 1, directCost)
            {
                Description = description,
            });

            await storageContext.SaveChanges();
            return product;
        }
    }
}