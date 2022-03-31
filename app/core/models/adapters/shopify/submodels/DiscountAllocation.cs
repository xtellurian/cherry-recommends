using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class DiscountAllocation
    {
        /// <summary>
        /// The discount amount allocated to the line (not sure why it is a string)
        /// </summary>
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// The index of the associated discount application in the order's discount_applications list.
        /// </summary>
        [JsonPropertyName("discount_application_index")]
        public long DiscountApplicationIndex { get; set; }

        /// <summary>
        /// The discount amount allocated to the line item in shop and presentment currencies.
        /// </summary>
        [JsonPropertyName("amount_set")]
        public PriceSet AmountSet { get; set; }
    }
}