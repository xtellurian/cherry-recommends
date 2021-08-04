using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{

    public partial class HubspotCrmCardResponse
    {
        public void AddFeatureValueCard(TrackedUserFeature featureValue)
        {
            this.Results ??= new List<Dictionary<string, object>>();
            Results.Add(new Dictionary<string, object>
            {
                { "value", featureValue.Value},
                { "objectId", featureValue.Id},
                { "title", featureValue.Feature.Name }
            });
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
