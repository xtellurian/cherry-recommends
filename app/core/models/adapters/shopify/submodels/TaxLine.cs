using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class TaxLine
    {
        /// <summary>
        /// The amount of tax to be charged.
        /// </summary>
        [JsonPropertyName("price")]
        public string Price { get; set; }

        /// <summary>
        /// The rate of tax to be applied.
        /// </summary>
        [JsonPropertyName("rate")]
        public string Rate { get; set; }

        /// <summary>
        /// The name of the tax.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The amount added to the order for this tax in shop and presentment currencies.
        /// </summary>
        [JsonPropertyName("price_set")]
        public PriceSet PriceSet { get; set; }
    }
}