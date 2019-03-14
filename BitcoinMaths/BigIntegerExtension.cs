using System;
using System.Linq;
using System.Numerics;

namespace BitcoinMaths
{
    public static class BigIntegerExtension
    {
        public static BigInteger Mod(this BigInteger x, BigInteger mod) => (x % mod + mod) % mod;

        public static byte[] ToByteArray(this BigInteger x, ByteArrayFormat format, int size = 0)
        {
            var littleEndianBytes = x.ToByteArray();
            var bigEndianBytes = littleEndianBytes.Reverse().ToArray();
            if ((format & ByteArrayFormat.Unsigned) == ByteArrayFormat.Unsigned)
            {
                if (x < 0)
                {
                    throw new InvalidOperationException("ByteArrayFormat.Unsigned can only be called on positive values.");
                }
                bigEndianBytes = bigEndianBytes.SkipWhile(b => b == 0).ToArray();
            }
            if (size > bigEndianBytes.Length)
            {
                var padding = new byte[size - bigEndianBytes.Length];
                bigEndianBytes = padding.Concat(bigEndianBytes).ToArray();
            }
            littleEndianBytes = bigEndianBytes.Reverse().ToArray();
            return ((format & ByteArrayFormat.BigEndian) > 0) ? bigEndianBytes : littleEndianBytes;
        }
    }
}
