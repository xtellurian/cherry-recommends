namespace SignalBox.Core
{
    public interface IHasher
    {
        string DefaultAlgorithm { get; }
        string Hash(string value);
        string Hash(string algorithm, string value);
    }
}