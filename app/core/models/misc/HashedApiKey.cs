using System;

namespace SignalBox.Core
{
    public class HashedApiKey : Entity
    {
        protected HashedApiKey() { }
        public HashedApiKey(string name, ApiKeyTypes apiKeyType, string algorithmName, string hashedKey, string scope)
        {
            this.AlgorithmName = algorithmName;
            this.ApiKeyType = apiKeyType;
            this.HashedKey = hashedKey;
            this.Scope = scope?.Trim(); // ensure there is no leading / trailing whitespace.
            this.Name = name;
        }
        public string Name { get; set; }
        public string AlgorithmName { get; set; }
        public string Scope { get; set; }
        public string HashedKey { get; set; }
        public DateTimeOffset? LastExchanged { get; set; }
        public int TotalExchanges { get; set; }
        public ApiKeyTypes? ApiKeyType { get; set; }
    }
}