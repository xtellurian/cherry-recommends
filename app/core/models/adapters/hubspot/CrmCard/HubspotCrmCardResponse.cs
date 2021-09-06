using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Adapters.Hubspot
{

    public partial class HubspotCrmCardResponse
    {
        private const string userFeaturesEntryKey = "commonUserId";
        private const string customerInformationTitle = "Customer Information";
        private const string recommendationTitle = "Recommendation";
        public void AddFeatureValueCard(TrackedUserFeatureBase featureValue)
        {
            this.Results ??= new List<Dictionary<string, object>>();
            var cardEntry = Results.FirstOrDefault(_ => _.GetValueOrDefault("title") as string == customerInformationTitle);
            cardEntry ??= new Dictionary<string, object>
            {
                { "title", customerInformationTitle},
                { userFeaturesEntryKey, featureValue.TrackedUser.CommonUserId},
                { "objectId", featureValue.Id},
                { "properties", new List<Dictionary<string,object>>() }
            };

            var properties = cardEntry["properties"] as List<Dictionary<string, object>>;
            if (featureValue.IsNumeric())
            {
                properties.Add(new Dictionary<string, object>
                {
                    { "label", featureValue.Feature.Name },
                    { "dataType", "NUMERIC" },
                    { "value", featureValue.NumericValue }
                });
            }
            else
            {
                properties.Add(new Dictionary<string, object>
                {
                    { "label", featureValue.Feature.Name },
                    { "dataType", "STRING" },
                    { "value", featureValue.StringValue }
                });
            }

            if (!this.Results.Any(_ => _.GetValueOrDefault("title") as string == customerInformationTitle))
            {
                this.Results.Add(cardEntry); // add if its new
            }
        }
        public void AddRecommendation(string baseUrl, RecommendationEntity recommendation)
        {
            this.Results ??= new List<Dictionary<string, object>>();
            var cardEntry = Results.FirstOrDefault(_ => _.GetValueOrDefault("title") as string == recommendationTitle);
            cardEntry ??= new Dictionary<string, object>
            {
                { "title", recommendationTitle},
                { userFeaturesEntryKey, recommendation.TrackedUser.CommonUserId},
                { "objectId", recommendation.Id},
                { "properties", new List<Dictionary<string,string>>() }
            };

            var properties = cardEntry["properties"] as List<Dictionary<string, string>>;

            if (recommendation.RecommenderType == Recommenders.RecommenderTypes.ParameterSet)
            {
                var parameterRecommendations = recommendation.GetOutput<ParameterSetRecommenderModelOutputV1>().RecommendedParameters;
                foreach (var kvp in parameterRecommendations)
                {
                    properties.Add(new Dictionary<string, string>
                {
                    { "label", $"{kvp.Key}" },
                    { "dataType", "STRING" },
                    { "value", kvp.Value.ToString() }
                });
                }
            }
            else if (recommendation.RecommenderType == Recommenders.RecommenderTypes.Product)
            {
                var productRecommendation = (ProductRecommendation)recommendation;

                properties.Add(new Dictionary<string, string>
                {
                    { "label", $"{productRecommendation.Recommender.Name}" },
                    { "dataType", "STRING" },
                    { "value", productRecommendation.Product.Name }
                });
            }

            // add actions to track good/bad recommendaion
            var actions = new List<object>(); // use object to enforce polymorphic serialization
            actions.Add(new ActionHookCrmCardAction
            {
                // this must be the absolute URI
                Uri = $"{baseUrl.TrimEnd('/')}/api/hubspotwebhooks/recommendations/{recommendation.RecommendationCorrelatorId}/outcomes/GOOD",
                Label = "Good recommendation",
            });
            actions.Add(new ActionHookCrmCardAction
            {
                // this must be the absolute URI
                Uri = $"{baseUrl.TrimEnd('/')}/api/hubspotwebhooks/recommendations/{recommendation.RecommendationCorrelatorId}/outcomes/BAD",
                Label = "Needs improvement",
            });

            cardEntry["actions"] = actions;

            if (!this.Results.Any(_ => _.GetValueOrDefault("title") as string == recommendationTitle))
            {
                this.Results.Add(cardEntry); // add if its new
            }

        }

        [JsonPropertyName("results")]
        public List<Dictionary<string, object>> Results { get; set; }

    }

    public static class Generator
    {
        public static HubspotCrmCardResponse GetResponse()
        {
            return new HubspotCrmCardResponse
            {
                Results = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {"title", "AFIRMS analysis"},
                        {"objectId" ,12345678},
                        {"archetype" , "Champion"},
                        {"healthScore" , "Good"}

                    }
                }
            };
        }
    }
}
