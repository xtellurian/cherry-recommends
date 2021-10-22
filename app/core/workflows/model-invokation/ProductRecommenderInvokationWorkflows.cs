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
                                    IHistoricTrackedUserFeatureStore historicFeatureStore,
                                    IProductStore productStore,
                                    IRecommendationCorrelatorStore correlatorStore,
                                    IProductRecommenderStore productRecommenderStore,
                                    IProductRecommendationStore productRecommendationStore)
                                     : base(storageContext, productRecommenderStore, historicFeatureStore, dateTimeProvider)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.modelClientFactory = modelClientFactory;
            this.trackedUserStore = trackedUserStore;
            this.productStore = productStore;
            this.correlatorStore = correlatorStore;
            this.productRecommenderStore = productRecommenderStore;
            this.productRecommendationStore = productRecommendationStore;
        }

        public async Task<ProductRecommendation> InvokeProductRecommender(
            ProductRecommender recommender,
            ProductRecommenderModelInputV1 input)
        {
            await productRecommenderStore.Load(recommender, _ => _.ModelRegistration);
            var invokationEntry = await base.StartTrackInvokation(recommender, input, saveOnComplete: false);
            var correlator = await correlatorStore.Create(new RecommendationCorrelator(recommender));
            await storageContext.SaveChanges(); // save the correlator and invokatin entry
            var context = new RecommendingContext(correlator, invokationEntry);

            try
            {
                if (string.IsNullOrEmpty(input.CommonUserId))
                {
                    throw new BadRequestException("ParameterSetRecommenderId is a required parameter.");
                }

                // enrich values from the touchpoint
                context.TrackedUser = await trackedUserStore.CreateIfNotExists(input.CommonUserId, $"Auto-created by Recommender {recommender.Name}");

                // load the features of the tracked user
                input.Features = await base.GetFeatures(context);

                IRecommenderModelClient<ProductRecommenderModelOutputV1> client;
                if (recommender.ModelRegistration == null)
                {
                    // load the products required for the random recommender
                    await productRecommenderStore.LoadMany(recommender, _ => _.Products);
                    // create a random recommender here.
                    client = await modelClientFactory.GetUnregisteredClient<ProductRecommenderModelOutputV1>(recommender);
                    logger.LogWarning($"Using unregistered model client for {recommender.Id}");
                }
                else if (recommender.ModelRegistration.ModelType != ModelTypes.ProductRecommenderV1)
                {
                    throw new BadRequestException("Model is not a ProductRecommenderV1");
                }
                else
                {
                    correlator.ModelRegistration = recommender.ModelRegistration;
                    client = await modelClientFactory
                       .GetClient<ProductRecommenderModelOutputV1>(recommender);
                }

                var output = await client.Invoke(recommender, context, input);

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

                return await HandleRecommendation(recommender, context, input, output);
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
                        context,
                        false,
                        message: $"Invoke failed for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId}",
                        modelResponse: modelResponseContent,
                        saveOnComplete: true);
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
                    return await HandleRecommendation(recommender, context, input, output);
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
                    return await HandleRecommendation(recommender, context, input, output);
                }
            }
        }

        private async Task<ProductRecommendation> HandleRecommendation(ProductRecommender recommender,
                                                                        RecommendingContext context,
                                                                        ProductRecommenderModelInputV1 input,
                                                                        ProductRecommenderModelOutputV1 output)
        {
            // now save the result

            var recommendation = new ProductRecommendation(recommender, context.TrackedUser, context.Correlator, output.Product);
            output.CorrelatorId = context?.Correlator.Id;
            recommendation.SetInput(input);
            recommendation.SetOutput(output);

            recommendation = await productRecommendationStore.Create(recommendation);
            await base.EndTrackInvokation(context,
                                          true,
                                          $"Invoked successfully for {context.TrackedUser?.Name ?? context.TrackedUser?.CommonId}",
                                          saveOnComplete: true);

            // set this after the context has been saved.
            output.CorrelatorId = context?.Correlator.Id;
            return recommendation;
        }
    }
}