namespace SignalBox.Core.Integrations
{
    public class ShopifyAppCredentials
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string[] Scopes { get; set; }
    }
}