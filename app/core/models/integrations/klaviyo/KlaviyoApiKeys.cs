namespace SignalBox.Core.Integrations
{
    public class KlaviyoApiKeys
    {
        public KlaviyoApiKeys(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}