using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{
    public class ActionHookCrmCardAction : CrmCardAction
    {
        [JsonPropertyName("type")]
        public override string Type { get; protected set; } = "ACTION_HOOK";
        [JsonPropertyName("httpMethod")]
        public string HttpMethod { get; set; } = "GET";
    }
}