using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    /// <summary> Contains the created integrated system with type Shopify </summary>
    public class ShopifyConnectSuccessDto
    {
        public IntegratedSystem IntegratedSystem { get; set; }
        public string ChargeConfirmationUrl { get; set; }
    }
}