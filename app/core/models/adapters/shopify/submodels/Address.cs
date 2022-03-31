using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    public class Address : ShopifyObjectBase
    {
        /// <summary>
        /// The mailing address.
        /// </summary>
        [JsonPropertyName("address1")]
        public string Address1 { get; set; }

        /// <summary>
        /// An additional field for the mailing address.
        /// </summary>
        [JsonPropertyName("address2")]
        public string Address2 { get; set; }

        /// <summary>
        /// The city.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// The company.
        /// </summary>
        [JsonPropertyName("company")]
        public string Company { get; set; }

        /// <summary>
        /// The country.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// The two-letter country code corresponding to the country.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// The normalized country name.
        /// </summary>
        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }

        /// <summary>
        /// Indicates whether this address is the default address.
        /// </summary>
        [JsonPropertyName("default")]
        public bool? Default { get; set; }

        /// <summary>
        /// The first name.
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name.
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// The latitude. Auto-populated by Shopify on the order's Billing/Shipping address.
        /// </summary>
        [JsonPropertyName("latitude")]
        public object Latitude { get; set; }

        /// <summary>
        /// The longitude. Auto-populated by Shopify on the order's Billing/Shipping address.
        /// </summary>
        [JsonPropertyName("longitude")]
        public object Longitude { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The phone number.
        /// </summary>
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// The province or state name
        /// </summary>
        [JsonPropertyName("province")]
        public string Province { get; set; }

        /// <summary>
        /// The two-letter province or state code.
        /// </summary>
        [JsonPropertyName("province_code")]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// The ZIP or postal code.
        /// </summary>
        [JsonPropertyName("zip")]
        public string Zip { get; set; }
    }
}