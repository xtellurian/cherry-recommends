namespace SignalBox.Core.Adapters.Shopify
{
    public interface IMutationResponseData
    {
        UserError[] UserErrors { get; set; }
    }
}