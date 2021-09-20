using System;

namespace SignalBox.Core
{
    public class HashedApiKey : Entity
    {
        protected HashedApiKey() { }
        public HashedApiKey(string name, string algorithmName, string hashedKey, string scope)
        {
            this.AlgorithmName = algorithmName;
            this.HashedKey = hashedKey;
            this.Scope = scope;
            this.Name = name;
        }
        public string Name { get; set; }
        public string AlgorithmName { get; set; }
        public string Scope { get; set; }
        public string HashedKey { get; set; }
        public DateTimeOffset? LastExchanged { get; set; }
        public int TotalExchanges { get; set; }
    }
}