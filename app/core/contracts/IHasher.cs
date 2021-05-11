namespace SignalBox.Core
{
    public interface IHasher
    {
        string Hash(string value);
        string AlgorithmName { get; }
    }
}