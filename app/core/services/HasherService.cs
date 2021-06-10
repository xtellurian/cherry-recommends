using System;
using System.Security.Cryptography;
using System.Text;

namespace SignalBox.Core
{
    public class Sha256HasherService : IHasher
    {
        public string DefaultAlgorithm => "SHA256"; // dont change this!

        public string Hash(string algorithm, string value)
        {
            switch (algorithm)
            {
                case "SHA256":
                    return GetHashSha256String(value);
                default:
                    throw new ArgumentException($"{algorithm} is an unknown hashing algorithm");
            }
        }

        public string Hash(string value)
        {
            // default to SHA256
            return GetHashSha256String(value);
        }

        private static string GetHashSha256String(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHashSha256(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private static byte[] GetHashSha256(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

    }
}