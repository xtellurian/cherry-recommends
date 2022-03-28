namespace SignalBox.Core.Integrations
{
    public class ShopifyStoreCredentials
    {
        private string shopifyUrl { get; set; }
        private string accessToken { get; set; }

        public ShopifyStoreCredentials(string shopifyUrl, string accessToken)
        {
            ShopifyUrl = shopifyUrl;
            AccessToken = accessToken;
        }

        public string ShopifyUrl
        {
            get => shopifyUrl;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    shopifyUrl = value;
                }
            }
        }
        public string AccessToken
        {
            get => accessToken;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    accessToken = value;
                }
            }
        }
    }
}