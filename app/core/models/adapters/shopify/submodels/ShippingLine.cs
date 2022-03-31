using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class ShippingLine
    {
        /// <summary>
        /// The carrier provided identifier.
        /// </summary>
        [JsonPropertyName("carrier_identifier")]
        public string CarrierIdentifier { get; set; }

        /// <summary>
        /// A reference to the shipping method.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// The phone number used for the shipment.
        /// </summary>
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// The price of this shipping method.
        /// </summary>
        [JsonPropertyName("price")]
        public string Price { get; set; }

        /// <summary>
        /// The discounted price of this shipping method.
        /// </summary>
        [JsonPropertyName("discounted_price")]
        public string DiscountedPrice { get; set; }

        /// <summary>
        /// An ordered list of amounts allocated by discount applications. Each discount allocation is associated to a particular discount application.
        /// </summary>
        [JsonPropertyName("discount_allocations")]
        public IEnumerable<DiscountAllocation> DiscountAllocations { get; set; }

        /// <summary>
        /// The source of the shipping method.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// The title of the shipping method.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// A list of <see cref="TaxLine"/> objects, each of which details the taxes applicable to this <see cref="ShippingLine"/>.
        /// </summary>
        [JsonPropertyName("tax_lines")]
        public IEnumerable<TaxLine> TaxLines { get; set; }

        /// <summary>
        /// The price of the shipping method in shop and presentment currencies.
        /// </summary>
        [JsonPropertyName("price_set")]
        public PriceSet PriceSet { get; set; }

        /// <summary>
        /// The price of the shipping method in both shop and presentment currencies after line-level discounts have been applied.
        /// </summary>
        [JsonPropertyName("discounted_price_set")]
        public PriceSet DiscountedPriceSet { get; set; }
    }
}