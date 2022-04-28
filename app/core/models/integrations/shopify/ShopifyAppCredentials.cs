namespace SignalBox.Core.Integrations
{
    public class ShopifyAppCredentials : IIntegratedSystemCredentials
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string[] Scopes { get; set; }
    }
}