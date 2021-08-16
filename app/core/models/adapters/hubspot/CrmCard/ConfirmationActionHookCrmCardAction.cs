using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{
    public class ConfirmationActionHookCrmCardAction : CrmCardAction
    {
        [JsonPropertyName("type")]
        public override string Type { get; protected set; } =  "CONFIRMATION_ACTION_HOOK";
        [JsonPropertyName("httpMethod")]
        public string HttpMethod { get; protected set; } = "GET";

        [JsonPropertyName("confirmationMessage")]
        public string ConfirmationMessage { get; set; }
        [JsonPropertyName("confirmButtonText")]
        public string ConfirmButtonText { get; set; }
        [JsonPropertyName("cancelButtonText")]
        public string CancelButtonText { get; set; }
    }
}