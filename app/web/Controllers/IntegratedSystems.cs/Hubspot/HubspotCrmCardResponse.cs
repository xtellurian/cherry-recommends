using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Web.Dto
{

    public partial class HubspotCrmCardResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<Dictionary<string, object>> Results { get; set; }

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
