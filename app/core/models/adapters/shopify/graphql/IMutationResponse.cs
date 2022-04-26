namespace SignalBox.Core.Adapters.Shopify
{
    public interface IMutationResponse<T> where T : IMutationResponseData
    {
        T Mutation { get; set; }
    }
}