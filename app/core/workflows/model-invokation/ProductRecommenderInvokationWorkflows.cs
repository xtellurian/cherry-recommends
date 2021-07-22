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

        public async Task<ProductRecommenderModelOutputV1> InvokeProductRecommender(long id, string version, ProductRecommenderModelInputV1 input)
        {
            var recommender = await productRecommenderStore.Read(id, _ => _.ModelRegistration);
            var invokationEntry = await base.StartTrackInvokation(recommender);
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

                // now save the result
                var correlator = await correlatorStore.Create(new RecommendationCorrelator());
                var recommendation = new ProductRecommendation(recommender, user, correlator, version, output.Product);
                recommendation.SetInput(input);
                recommendation.SetOutput(output);
                // todo the recommendation store
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
            catch (ModelInvokationException modelEx)
            {
                // TODO: return a default product on error
                logger.LogError("Error invoking recommender", modelEx);
                await base.EndTrackInvokation(
                    invokationEntry,
                    false,
                    user,
                    null,
                    $"Invoke failed for {user?.Name ?? user?.CommonId}",
                    modelEx.ModelResponseContent,
                    true);
                throw; // rethrow the error to propagae to calling client
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error invoking recommender", ex);
                await base.EndTrackInvokation(
                    invokationEntry,
                    false,
                    user,
                    null,
                    $"Invoke failed for {user?.Name ?? user?.CommonId}",
                    null,
                    true);
                throw; // rethrow the error to propagae to calling client
            }
        }
    }
}