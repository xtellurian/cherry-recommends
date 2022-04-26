using System;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Adapters.Shopify
{
    /// <summary>
    /// An object representing a recurring application charge (i.e. a monthly subscription to your Shopify application).
    /// </summary>
    public class ShopifyRecurringCharge : ShopifyObjectBase
    {
        [JsonConstructor]
        public ShopifyRecurringCharge()
        { }
        // useful constructor with minimum required info
        public ShopifyRecurringCharge(string name, decimal price, bool test, int trialDays)
        {
            Name = name;
            Price = price;
            Test = test;
            TrialDays = trialDays;
        }

        /// <summary>
        /// The date and time when the customer activated the <see cref="RecurringCharge"/>. Will be null if the charge
        /// has not been activated.
        /// </summary>
        [JsonPropertyName("activated_on")]
        public DateTimeOffset? ActivatedOn { get; set; }
        /// <summary>
        /// The date and time when the customer will be billed. Will be null if the charge has not been activated by the customer.
        /// </summary>
        [JsonPropertyName("billing_on")]
        public DateTimeOffset? BillingOn { get; set; }
        /// <summary>
        /// The capped amount is the limit a customer can be charged for usage based billing.
        /// </summary>
        [JsonPropertyName("capped_amount")]
        public decimal? CappedAmount { get; set; }
        /// <summary>
        /// The date and time when the customer cancelled their recurring application charge. Will be null if the charge has not
        /// been cancelled.
        /// </summary>
        [JsonPropertyName("cancelled_on")]
        public DateTimeOffset? CancelledOn { get; set; }
        /// <summary>
        /// The URL that the customer should be sent to, to accept or decline the recurring application charge.
        /// </summary>
        [JsonPropertyName("confirmation_url")]
        public string ConfirmationUrl { get; set; }
        /// <summary>
        /// The date and time when the recurring application charge was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        /// <summary>
        /// The name of the recurring application charge.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// The price of the recurring application charge.
        /// </summary>
        /// <remarks>Shopify returns this as a string, but JSON.net should be able to convert it to a decimal.</remarks>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// The URL the customer is sent to once they accept/decline a charge.
        /// </summary>
        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; }
        /// <summary>
        ///  Known values are 'pending', 'accepted', 'active', 'cancelled', 'declined' and 'expired'.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
        /// <summary>
        /// States the terms and conditions of usage based billing charges. Must be present in order to create usage charges. These are presented to the merchant when they approve the usage charges for your app.
        /// </summary>
        [JsonPropertyName("terms")]
        public string Terms { get; set; }
        /// <summary>
        /// States whether or not the application charge is a test transaction.
        /// </summary>
        /// <remarks>Valid values are 'true' or null. Needs a special converter to convert null to false and vice-versa.</remarks>
        [JsonPropertyName("test")]
        public bool? Test { get; set; }
        /// <summary>
        /// Number of days that the customer is eligible for a free trial.
        /// </summary>
        [JsonPropertyName("trial_days")]
        public int? TrialDays { get; set; }
        /// <summary>
        /// The date and time when the free trial ends. Will be null if the charge has not been accepted.
        /// </summary>
        [JsonPropertyName("trial_ends_on")]
        public DateTimeOffset? TrialEndsOn { get; set; }
        /// <summary>
        /// The date and time when the recurring application charge was last updated.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
