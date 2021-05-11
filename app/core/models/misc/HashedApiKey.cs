namespace SignalBox.Core
{
    public class HashedApiKey : NamedEntity
    {
        public HashedApiKey(string name, string algorithmName, string hashedKey): base(name)
        {
            AlgorithmName = algorithmName;
            HashedKey = hashedKey;
        }

        public string AlgorithmName { get; set; }
        public string HashedKey { get; set; }
    }
}