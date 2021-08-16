using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Hubspot
{
    public class CrmCardResponseWithPrimaryAction<TPrimaryAction> : HubspotCrmCardResponse where TPrimaryAction : CrmCardAction
    {
        [JsonPropertyName("primaryAction")]
        public TPrimaryAction PrimaryAction { get; set; }
    }
}