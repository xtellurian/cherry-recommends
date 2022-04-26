namespace SignalBox.Core.Integrations
{
    /// <summary>
    /// A configuration class.
    /// Contains information for Cherry Billing via Shopify Billing API
    /// </summary>
    public class ShopifyBilling
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Test { get; set; }
        public int TrialDays { get; set; }
    }
}
