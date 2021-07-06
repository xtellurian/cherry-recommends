using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ProductRecommenderWorkflows
    {
        private readonly IStorageContext storageContext;
        private readonly IProductRecommenderStore store;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly IProductStore productStore;

        public ProductRecommenderWorkflows(IStorageContext storageContext,
                                                IProductRecommenderStore store,
                                                IModelRegistrationStore modelRegistrationStore,
                                                ITouchpointStore touchpointStore,
                                                IProductStore productStore)
        {
            this.storageContext = storageContext;
            this.store = store;
            this.modelRegistrationStore = modelRegistrationStore;
            this.touchpointStore = touchpointStore;
            this.productStore = productStore;
        }

        public async Task<ProductRecommender> CreateProductRecommender(CreateCommonEntityModel common, string touchpointId, IEnumerable<string> productCommonIds)
        {
            Touchpoint? touchpoint = null;
            if (touchpointId != null)
            {
                touchpoint = await touchpointStore.ReadFromCommonId(touchpointId);
            }

            if (productCommonIds != null && productCommonIds.Any())
            {
                var products = new List<Product>();
                foreach (var p in productCommonIds)
                {
                    products.Add(await productStore.ReadFromCommonId(p));
                }

                var recommender = await store.Create(new ProductRecommender(common.CommonId, common.Name, touchpoint, products));
                await storageContext.SaveChanges();
                return recommender;
            }
            else
            {
                var recommender = await store.Create(new ProductRecommender(common.CommonId, common.Name, touchpoint, null));
                await storageContext.SaveChanges();
                return recommender;
            }
        }

        public async Task<ModelRegistration> LinkRegisteredModel(ProductRecommender recommender, long modelId)
        {
            var model = await modelRegistrationStore.Read(modelId);
            recommender.ModelRegistration = model;
            await storageContext.SaveChanges();
            return model;
        }
    }
}