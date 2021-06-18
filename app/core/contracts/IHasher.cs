namespace SignalBox.Core
{
    public interface IHasher
    {
        string DefaultAlgorithm { get; }
        byte[] HashToBytes(HashingAlgorithms algorithm, string value);
        string Hash(string value);
        string Hash(HashingAlgorithms algorithm, string value);
    }

    public enum HashingAlgorithms
    {
        SHA256
    }
}