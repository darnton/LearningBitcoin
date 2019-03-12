using System;
using System.Linq;

namespace BitcoinMaths
{
    public static class Serialisation
    {
        public const byte SEC_MARKER_MIN = 2;
        public const byte SEC_MARKER_MAX = 4;
        public const byte SEC_MARKER_COMPRESSED_EVEN = 2;
        public const byte SEC_MARKER_COMPRESSED_ODD = 3;
        public const byte SEC_MARKER_UNCOMPRESSED = 4;

        public const int SEC_LENGTH_UNCOMPRESSED = 65;
        public const int SEC_LENGTH_COMPRESSED = 33;

        public static byte[] GetBytesFromHex(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }

        public static string GetHexFromBytes(byte[] buffer)
        {
            return string.Join("", buffer.Select(b => b.ToString("x2")));
        }
    }
}
