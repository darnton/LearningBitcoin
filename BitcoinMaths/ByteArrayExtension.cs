using System.Linq;
using System.Numerics;

namespace BitcoinMaths
{
    public static class ByteArrayExtension
    {
        public static BigInteger ToUnsignedBigInteger(this byte[] buffer)
        {
            //Little-endian byte order.
            return new BigInteger(buffer.Reverse().Concat(new byte[] { 0 }).ToArray());
        }
    }
}
