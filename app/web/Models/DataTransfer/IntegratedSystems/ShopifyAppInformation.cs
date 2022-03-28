namespace SignalBox.Web.Dto
{
    public class ShopifyAppInformation
    {
        public ShopifyAppInformation(string apiKey, string[] scopes)
        {
            ApiKey = apiKey;
            Scopes = scopes;
        }

        public string ApiKey { get; set; }
        public string[] Scopes { get; set; }
    }
}