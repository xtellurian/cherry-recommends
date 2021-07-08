using System;

namespace SignalBox.Core
{
    internal static class ByteArrayExtensions
    {

        public static string ToHexString(this byte[] bytes, Casing casing = Casing.Upper)
        {
            Span<char> result = stackalloc char[0];
            if (bytes.Length > 16)
            {
                var array = new char[bytes.Length * 2];
                result = array.AsSpan();
            }
            else
            {
                result = stackalloc char[bytes.Length * 2];
            }

            int pos = 0;
            foreach (byte b in bytes)
            {
                ToCharsBuffer(b, result, pos, casing);
                pos += 2;
            }

            return result.ToString();
        }

        private static void ToCharsBuffer(byte value, Span<char> buffer, int startingIndex = 0, Casing casing = Casing.Upper)
        {
            uint difference = (((uint)value & 0xF0U) << 4) + ((uint)value & 0x0FU) - 0x8989U;
            uint packedResult = ((((uint)(-(int)difference) & 0x7070U) >> 4) + difference + 0xB9B9U) | (uint)casing;

            buffer[startingIndex + 1] = (char)(packedResult & 0xFF);
            buffer[startingIndex] = (char)(packedResult >> 8);
        }
    }

    public enum Casing : uint
    {
        // Output [ '0' .. '9' ] and [ 'A' .. 'F' ].
        Upper = 0,

        // Output [ '0' .. '9' ] and [ 'a' .. 'f' ].
        Lower = 0x2020U,
    }
}