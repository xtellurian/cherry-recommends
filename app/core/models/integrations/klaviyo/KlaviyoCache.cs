namespace SignalBox.Core.Integrations
{
    public class KlaviyoCache : IIntegratedSystemCache
    {
        public KlaviyoCache(KlaviyoApiKeys apiKeys)
        {
            ApiKeys = apiKeys;
        }

        public KlaviyoApiKeys ApiKeys { get; }
    }
}