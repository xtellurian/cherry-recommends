using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    /// <summary>
    /// An object representing a Shopify fulfillment.
    /// </summary>
    public class Fulfillment : ShopifyObjectBase
    {
        /// <summary>
        /// The date and time when the fulfillment was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// A historical record of each item in the fulfillment.
        /// </summary>
        [JsonPropertyName("line_items")]
        public IEnumerable<LineItem> LineItems { get; set; }

        /// <summary>
        /// The unique numeric identifier for the order.
        /// </summary>
        [JsonPropertyName("order_id")]
        public long? OrderId { get; set; }

        /// <summary>
        /// A textfield with information about the receipt.
        /// </summary>
        [JsonPropertyName("receipt")]
        public object Receipt { get; set; }

        /// <summary>
        /// The status of the fulfillment. Valid values are 'pending', 'open', 'success', 'cancelled',
        /// 'error' and 'failure'.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// The unique identifier of the location that the fulfillment should be processed for.
        /// </summary>
        [JsonPropertyName("location_id")]
        public long? LocationId { get; set; }


        /// <summary>
        /// This property is undocumented by Shopify. It appears to be the customer's email address
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// A flag indicating whether the customer should be notified. If set to true, an email will be
        /// sent when the fulfillment is created or updated. The default value is false for fulfillments
        /// on any orders created initially through the API. For all other orders, the default value is true.
        /// </summary>
        [JsonPropertyName("notify_customer")]
        public bool? NotifyCustomer { get; set; }


        /// <summary>
        /// This property is undocumented by Shopify. It appears to be the shipping address of the order
        /// </summary>
        [JsonPropertyName("destination")]
        public Address Destination { get; set; }

        /// <summary>
        /// The name of the shipping company.
        /// </summary>
        [JsonPropertyName("tracking_company")]
        public string TrackingCompany { get; set; }

        /// <summary>
        /// The shipping number, provided by the shipping company. If multiple tracking numbers
        /// exist (<see cref="TrackingNumbers"/>), returns the first number.
        /// </summary>
        [JsonPropertyName("tracking_number")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// A list of shipping numbers, provided by the shipping company. May be null.
        /// </summary>
        [JsonPropertyName("tracking_numbers")]
        public IEnumerable<string> TrackingNumbers { get; set; }

        /// <summary>
        /// The tracking url, provided by the shipping company. May be null. If multiple tracking URLs
        /// exist (<see cref="TrackingUrls"/>), returns the first URL.
        /// </summary>
        [JsonPropertyName("tracking_url")]
        public string TrackingUrl { get; set; }

        /// <summary>
        /// An array of one or more tracking urls, provided by the shipping company. May be null.
        /// </summary>
        [JsonPropertyName("tracking_urls")]
        public IEnumerable<string> TrackingUrls { get; set; }

        /// <summary>
        /// The date and time when the fulfillment was last modified.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// States the name of the inventory management service.
        /// </summary>
        [JsonPropertyName("variant_inventory_management")]
        public string VariantInventoryManagement { get; set; }

        [JsonPropertyName("service")]
        /// <summary>
        /// This property is undocumented by Shopify.
        /// </summary>
        public string Service { get; set; }

        [JsonPropertyName("shipment_status")]
        /// <summary>
        /// This property is undocumented by Shopify.
        /// </summary>
        public string ShipmentStatus { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The address of the fulfillment location
        /// </summary>
        [JsonPropertyName("origin_address")]
        public Address OriginAddress { get; set; }
    }
}