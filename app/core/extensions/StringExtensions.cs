using System;

namespace SignalBox.Core
{
    public static class StringExtensions
    {
        public static string ToBase64Encoded(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        
        public static string ToBase64Encoded(this Guid guid)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(guid.ToString());
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}