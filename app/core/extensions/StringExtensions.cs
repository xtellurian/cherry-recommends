using System;
using System.Linq;

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

        public static bool ContainsOnlyLowercaseAlphaNumeric(this string value, params char[] symbols)
        {
            foreach (var c in value)
            {
                if (char.IsLetter(c) && char.IsLower(c))
                {
                    continue;
                }
                else if (char.IsNumber(c))
                {
                    return true;
                }
                else if (char.IsUpper(c))
                {
                    return false;
                }
                else if (symbols.Contains(c))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            // default to true, since no char violated the rule
            return true;
        }

        public static bool ContainsOnlyAlphaNumeric(this string value, params char[] symbols)
        {
            foreach (var c in value)
            {
                if (char.IsLetter(c))
                {
                    continue;
                }
                else if (char.IsNumber(c))
                {
                    return true;
                }
                else if (symbols.Contains(c))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            // default to true, since no char violated the rule
            return true;
        }
    }
}