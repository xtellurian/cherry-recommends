using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class LineItemDuty : ShopifyObjectBase
    {
        [JsonPropertyName("harmonized_system_code")]
        public string HarmonizedSystemCode { get; set; }

        [JsonPropertyName("country_code_of_origin")]
        public string CountryCodeOfOrigin { get; set; }

        [JsonPropertyName("shop_money")]
        public Price ShopMoney { get; set; }

        [JsonPropertyName("presentment_money")]
        public Price PresentmentMoney { get; set; }

        [JsonPropertyName("tax_lines")]
        public IEnumerable<TaxLine> TaxLines { get; set; }
    }
}