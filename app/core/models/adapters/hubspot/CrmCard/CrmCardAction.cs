using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{
    public abstract class CrmCardAction
    {
        public CrmCardAction()
        {
            AssociatedObjectProperties = new List<string>
            {
                "userId",
                "userEmail",
                "associatedObjectId",
                "associatedObjectType",
                "portalId"
            };
        }
        [JsonPropertyName("type")]
        public abstract string Type { get; protected set; }
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("associatedObjectProperties")]
        public List<string> AssociatedObjectProperties { get; set; }
    }
}