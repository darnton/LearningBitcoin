using System;
using System.Linq;
using System.Numerics;

namespace BitcoinMaths
{
    public static class ByteArrayExtension
    {
        public static byte[] Segment(this byte[] buffer, int offset, int count)
        {
            return new ArraySegment<byte>(buffer, offset, count).ToArray();
        }

        public static byte[] PrefixZeroes(this byte[] buffer, int count)
        {
            if (count == 0) return buffer;

            var result = new byte[buffer.Length + count];
            buffer.CopyTo(result, count);
            return result;
        }

        public static byte[] PostfixZeroes(this byte[] buffer, int count)
        {
            if (count == 0) return buffer;

            var result = new byte[buffer.Length + count];
            buffer.CopyTo(result, 0);
            return result;
        }

        public static BigInteger ToBigInteger(this byte[] buffer, ByteArrayFormat format = ByteArrayFormat.LittleEndianSigned)
        {
            if ((format & ByteArrayFormat.BigEndian) == ByteArrayFormat.BigEndian)
            {
                buffer = buffer.Reverse().ToArray();
            }
            if ((format & ByteArrayFormat.Unsigned) == ByteArrayFormat.Unsigned)
            {
                buffer = buffer.PostfixZeroes(1);
            }
            return new BigInteger(buffer);
        }
    }
}
