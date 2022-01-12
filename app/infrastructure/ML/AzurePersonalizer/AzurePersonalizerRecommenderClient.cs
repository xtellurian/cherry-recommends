// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Microsoft.Azure.CognitiveServices.Personalizer;
// using Microsoft.Azure.CognitiveServices.Personalizer.Models;
// using SignalBox.Core;
// using SignalBox.Core.Recommenders;

// namespace SignalBox.Infrastructure.ML.Azure
// {
//     public class AzurePersonalizerRecommenderClient :
//         IRecommenderModelClient<ProductRecommenderModelOutputV1>,
//         IRecommenderModelRewardClient
//     {
//         public AzurePersonalizerRecommenderClient(HttpClient httpClient,
//                                                   ITelemetry telemetry)
//         {
//             this.httpClient = httpClient;
//             this.recommenderStore = recommenderStore;
//             this.telemetry = telemetry;
//             this.productStore = productStore;
//         }

//         // private static readonly string ApiKey = "5610ca7dee3d4ce7b5b04b757d6d5327";
//         // private static readonly string ServiceEndpoint = "https://rian-test-personalizer.cognitiveservices.azure.com/";
//         private readonly HttpClient httpClient;
//         private readonly IProductRecommenderStore recommenderStore;
//         private readonly ITelemetry telemetry;
//         private readonly IProductStore productStore;

//         private static PersonalizerClient InitializePersonalizerClient(HttpClient httpClient, ModelRegistration registration)
//         {
//             if (registration == null)
//             {
//                 throw new ConfigurationException($"Model Registration was null");
//             }
//             else if (registration.ScoringUrl == null)
//             {
//                 throw new ConfigurationException($"Scoring URL of Personaliser was null");
//             }
//             else if (registration.Key == null)
//             {
//                 throw new ConfigurationException($"Key of Personaliser was null");
//             }

//             PersonalizerClient client = new PersonalizerClient(
//                 new ApiKeyServiceClientCredentials(registration.Key), httpClient, false)
//             { Endpoint = registration.ScoringUrl };

//             return client;
//         }
//         public async Task<ProductRecommenderModelOutputV1> Invoke(IRecommender recommender, RecommendingContext recommendingContext, IModelInput input)
//         {
//             var client = InitializePersonalizerClient(httpClient, recommender.ModelRegistration);

//             ProductRecommender productRecommender = (ProductRecommender)recommender;
//             await recommenderStore.LoadMany(productRecommender, _ => _.Products);
//             await recommenderStore.Load(productRecommender, _ => _.DefaultProduct);

//             var products = productRecommender.Products;
//             if (products == null || !products.Any())
//             {
//                 var productQuery = await productStore.Query(1);
//                 products = productQuery.Items.ToList();
//             }

//             var rankableActions = ToRankableActions(products);

//             var currentContext = new List<object>();
//             // add features to the context
//             if (input.Features != null && input.Features.Any())
//             {
//                 foreach (var f in input.Features)
//                 {
//                     currentContext.Add(f);
//                 }
//             }

//             // add incoming arguments to the context (arguments overwrite features)
//             if (input.Arguments != null && input.Arguments.Any())
//             {
//                 foreach (var a in input.Arguments)
//                 {
//                     currentContext.Add(a);
//                 }
//             }

//             var personalizerEventId = recommendingContext.Correlator.Id.ToString();
//             var rank = await client.RankAsync(new RankRequest(rankableActions,
//                                                               currentContext,
//                                                               excludedActions: null,
//                                                               eventId: personalizerEventId));

//             var maxProb = rank.Ranking.Max(_ => _.Probability);
//             telemetry.TrackEvent("Models.AzurePersonalizer.Rank", new Dictionary<string, string>
//             {
//                 {"MaxProbability", maxProb.ToString()}
//             });

//             return new ProductRecommenderModelOutputV1
//             {
//                 ProductCommonId = rank.Ranking.OrderByDescending(_ => _.Probability).First().Id,
//             };
//         }

//         private IList<RankableAction> ToRankableActions(IEnumerable<Product> products)
//         {
//             return products.Select(p => new RankableAction
//             {
//                 Id = p.CommonId,
//                 Features =
//                 new List<object>() { new { name = p.Name, listPrice = p.ListPrice, directCost = p.DirectCost, description = p.Description } }
//             }).ToList();
//         }

//         public async Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action)
//         {
//             var personalizerEventId = action.RecommendationCorrelatorId?.ToString();
//             if (action.HasReward() && personalizerEventId != null)
//             {
//                 var client = InitializePersonalizerClient(httpClient, recommender.ModelRegistration);
//                 if (action.FeedbackScore.HasValue)
//                 {
//                     var rewardValue = action.FeedbackScore.Value / (context.NormaliseToMaximum ?? 1);
//                     await client.RewardAsync(personalizerEventId, new RewardRequest(rewardValue));
//                 }
//                 if (action.AssociatedRevenue.HasValue)
//                 {
//                     var rewardValue = action.AssociatedRevenue.Value / (context.NormaliseToMaximum ?? 1);
//                     await client.RewardAsync(personalizerEventId, new RewardRequest(rewardValue));
//                 }
//             }
//         }
//     }
// }