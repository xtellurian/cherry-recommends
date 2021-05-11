using System.Security.Cryptography;
using System.Text;

namespace SignalBox.Core
{
    public class Sha256HasherService : IHasher
    {
        public string AlgorithmName => "SHA256"; // dont change this!

        public string Hash(string value)
        {
            return GetHashString(value);
        }

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}