using System;
using System.Linq;
using System.Numerics;

namespace BitcoinMaths
{
    public static class BigIntegerExtension
    {
        public static BigInteger Mod(this BigInteger x, BigInteger mod) => (x % mod + mod) % mod;

        public static byte[] ToByteArrayUnsignedBigEndian(this BigInteger x, int size = 0)
        {
            if (x < 0)
            {
                throw new InvalidOperationException("ToByteArrayUnsignedBigEndian can only be called on positive values.");
            }
            var result = x.ToByteArray().Reverse().SkipWhile(b => b == 0).ToArray();
            if(size > result.Length)
            {
                var padding = new byte[size - result.Length];
                result = padding.Concat(result).ToArray();
            }
            return result;
        }
    }
}
