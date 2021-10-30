using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ProductRecommenderWorkflows : RecommenderWorkflowBase<ProductRecommender>
    {
        private readonly IStorageContext storageContext;
        private readonly IProductRecommendationStore recommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly IProductStore productStore;

        public ProductRecommenderWorkflows(IStorageContext storageContext,
                                                IProductRecommenderStore store,
                                                IProductRecommendationStore recommendationStore,
                                                IIntegratedSystemStore systemStore,
                                                IModelRegistrationStore modelRegistrationStore,
                                                ITouchpointStore touchpointStore,
                                                IProductStore productStore) : base(store, systemStore)
        {
            this.storageContext = storageContext;
            this.recommendationStore = recommendationStore;
            this.modelRegistrationStore = modelRegistrationStore;
            this.touchpointStore = touchpointStore;
            this.productStore = productStore;
        }

        public async Task<ProductRecommender> CloneProductRecommender(CreateCommonEntityModel common, ProductRecommender from)
        {
            await store.Load(from, _ => _.DefaultProduct);
            await store.LoadMany(from, _ => _.Products);
            return await CreateProductRecommender(common,
                                                  from.DefaultProduct?.CommonId,
                                                  from.Products?.Select(_ => _.CommonId),
                                                  from.Settings ?? new RecommenderSettings());
        }

        public async Task<ProductRecommender> CreateProductRecommender(CreateCommonEntityModel common,
                                                                       string? defaultProductId,
                                                                       IEnumerable<string>? productCommonIds,
                                                                       RecommenderSettings settings)
        {
            Product? defaultProduct = null;
            if (!string.IsNullOrEmpty(defaultProductId))
            {
                defaultProduct = await productStore.GetEntity(defaultProductId);
            }

            if (productCommonIds != null && productCommonIds.Any())
            {
                var products = new List<Product>();
                foreach (var p in productCommonIds)
                {
                    products.Add(await productStore.ReadFromCommonId(p));
                }

                var recommender = await store.Create(
                    new ProductRecommender(common.CommonId, common.Name, defaultProduct, products, null, settings));
                await storageContext.SaveChanges();
                return recommender;
            }
            else
            {
                var recommender = await store.Create(
                    new ProductRecommender(common.CommonId, common.Name, defaultProduct, null, null, settings));
                await storageContext.SaveChanges();
                return recommender;
            }
        }

        public async Task<Product> SetDefaultProduct(ProductRecommender recommender, string productId)
        {
            var product = await productStore.GetEntity(productId);
            recommender.DefaultProduct = product;
            await store.Update(recommender);
            await storageContext.SaveChanges();
            return product;
        }

        public async Task<Paginated<ProductRecommendation>> QueryRecommendations(long recommenderId, int page)
        {
            return await recommendationStore.QueryForRecommender(page, recommenderId);
        }

        public async Task<ModelRegistration> LinkRegisteredModel(ProductRecommender recommender, long modelId)
        {
            var model = await modelRegistrationStore.Read(modelId);
            if (model.ModelType == ModelTypes.ProductRecommenderV1)
            {
                recommender.ModelRegistration = model;
                await storageContext.SaveChanges();
                return model;
            }
            else
            {
                throw new BadRequestException($"Model of type {model.ModelType} can't be linked to a Product Recommender");
            }
        }
    }
}