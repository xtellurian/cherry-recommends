using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public class ProductRecommenderInvokationWorkflows : RecommenderInvokationWorkflowBase<ProductRecommender>, IWorkflow
    {
        private readonly ILogger<ProductRecommenderInvokationWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IRecommenderModelClientFactory modelClientFactory;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly IProductStore productStore;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IProductRecommenderStore productRecommenderStore;
        private readonly IProductRecommendationStore productRecommendationStore;

        public ProductRecommenderInvokationWorkflows(ILogger<ProductRecommenderInvokationWorkflows> logger,
                                    IStorageContext storageContext,
                                    IDateTimeProvider dateTimeProvider,
                                    IRecommenderModelClientFactory modelClientFactory,
                                    ITrackedUserStore trackedUserStore,
                                    ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                    ITouchpointStore touchpointStore,
                                    IProductStore productStore,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IProductRecommenderStore productRecommenderStore,
                                    IProductRecommendationStore productRecommendationStore)
                                     : base(storageContext, productRecommenderStore, dateTimeProvider)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.modelClientFactory = modelClientFactory;
            this.trackedUserStore = trackedUserStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
            this.touchpointStore = touchpointStore;
            this.productStore = productStore;
            this.correlatorStore = correlatorStore;
            this.productRecommenderStore = productRecommenderStore;
            this.productRecommendationStore = productRecommendationStore;
        }

        public async Task<ProductRecommenderModelOutputV1> InvokeProductRecommender(
            ProductRecommender recommender,
            string version,
            ProductRecommenderModelInputV1 input)
        {
            await productRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            var invokationEntry = await base.StartTrackInvokation(recommender, input?.CommonUserId);
            TrackedUser user = null;
            try
            {
                if (string.IsNullOrEmpty(input.CommonUserId))
                {
                    throw new BadRequestException("ParameterSetRecommenderId is a required parameter.");
                }

                input.Touchpoint ??= recommender.CommonId;
                // enrich values from the touchpoint
                var touchpoint = await touchpointStore.ReadFromCommonId(input.Touchpoint);
                user = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");
                invokationEntry.LogMessage("Create or Update Tracked User");
                if (await trackedUserTouchpointStore.TouchpointExists(user, touchpoint))
                {
                    var tpValues = await trackedUserTouchpointStore.ReadTouchpoint(user, touchpoint);
                    input.Arguments = tpValues.Values;
                }
                else
                {
                    input.Arguments = null;
                }

                IRecommenderModelClient<ProductRecommenderModelInputV1, ProductRecommenderModelOutputV1> client;
                if (recommender.ModelRegistration == null)
                {
                    // load the products required for the random recommender
                    await productRecommenderStore.LoadMany(recommender, _ => _.Products);
                    // create a random recommender here.
                    client = await modelClientFactory.GetUnregisteredClient<ProductRecommenderModelInputV1, ProductRecommenderModelOutputV1>(recommender);
                    logger.LogWarning($"Using unregistered model client for {recommender.Id}");
                }
                else if (recommender.ModelRegistration.ModelType != ModelTypes.ProductRecommenderV1)
                {
                    throw new BadRequestException("Model is not a ProductRecommenderV1");
                }
                else
                {
                    client = await modelClientFactory
                       .GetClient<ProductRecommenderModelInputV1, ProductRecommenderModelOutputV1>(recommender);
                }

                var output = await client.Invoke(recommender, version, input);

                // load the product
                if (output.ProductId.HasValue)
                {
                    output.Product ??= await productStore.Read(output.ProductId.Value);
                }
                else if (!string.IsNullOrEmpty(output.ProductCommonId))
                {
                    output.Product ??= await productStore.ReadFromCommonId(output.ProductCommonId);
                }
                else if (output.Product == null)
                {
                    throw new ModelInvokationException("The model did not return a product.");
                }

                return await HandleRecommendation(recommender, version, input, invokationEntry, user, output);
            }
            catch (System.Exception ex)
            {
                string modelResponseContent = null;
                if (ex is ModelInvokationException modelEx)
                {
                    logger.LogError("Error invoking recommender", modelEx);
                    modelResponseContent = modelEx.ModelResponseContent;
                }
                else
                {
                    logger.LogError("Error invoking recommender model");
                }

                if (recommender.ShouldThrowOnBadInput())
                {
                    await base.EndTrackInvokation(
                        invokationEntry,
                        false,
                        user,
                        null,
                        $"Invoke failed for {user?.Name ?? user?.CommonId}",
                        modelResponseContent,
                        true);
                    throw; // rethrow the error to propagate to calling client
                }
                else if (recommender.DefaultProductId != null)
                {
                    // case: default product and the model returned error
                    await productRecommenderStore.Load(recommender, _ => _.DefaultProduct);
                    invokationEntry.LogMessage($"Model Error. Fallback to default product");
                    var output = new ProductRecommenderModelOutputV1
                    {
                        ProductId = recommender.DefaultProductId,
                        ProductCommonId = recommender.DefaultProduct.CommonId,
                        Product = recommender.DefaultProduct
                    };
                    return await HandleRecommendation(recommender, version, input, invokationEntry, user, output);
                }
                else
                {
                    // case: no default and the model returned error
                    var someProducts = await productStore.Query(1);
                    var product = someProducts.Items.First();
                    invokationEntry.LogMessage($"Model Error. Fallback to top product {product.CommonId}");
                    var output = new ProductRecommenderModelOutputV1
                    {
                        ProductId = recommender.DefaultProductId,
                        ProductCommonId = recommender.DefaultProduct.CommonId,
                        Product = recommender.DefaultProduct
                    };
                    return await HandleRecommendation(recommender, version, input, invokationEntry, user, output);
                }
            }
        }

        private async Task<ProductRecommenderModelOutputV1> HandleRecommendation(ProductRecommender recommender, string version, ProductRecommenderModelInputV1 input, InvokationLogEntry invokationEntry, TrackedUser user, ProductRecommenderModelOutputV1 output)
        {
            // now save the result
            var correlator = await correlatorStore.Create(new RecommendationCorrelator());
            var recommendation = new ProductRecommendation(recommender, user, correlator, version, output.Product);
            recommendation.SetInput(input);
            recommendation.SetOutput(output);

            recommendation = await productRecommendationStore.Create(recommendation);
            await base.EndTrackInvokation(invokationEntry,
                                          true,
                                          user,
                                          correlator,
                                          $"Invoked successfully for {user.Name ?? user.CommonId}",
                                          null,
                                          false);

            await storageContext.SaveChanges();

            // set this after the context has been saved.
            output.CorrelatorId = correlator.Id;
            return output;
        }
    }
}