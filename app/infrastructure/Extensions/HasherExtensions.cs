using System;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public static class HasherExtensions
    {
        public static string HashHttpRequestForWebhookValidation(this IHasher hasher, string clientSecret, string requestBody)
        {
            var valueToHash = $"{clientSecret}{requestBody}";
            var hashedBytes = hasher.HashToBytes(HashingAlgorithms.SHA256, valueToHash);
            var hashed = Convert.ToHexString(hashedBytes).ToLower();
            return hashed;
        }

        public static string HashHttpRequestForWebhookValidation(this IHasher hasher, string clientSecret, string method, string uri)
        {
            var valueToHash = $"{clientSecret}{method}{uri}";
            var hashedBytes = hasher.HashToBytes(HashingAlgorithms.SHA256, valueToHash);
            var hashed = Convert.ToHexString(hashedBytes).ToLower();
            return hashed;
        }
    }
}