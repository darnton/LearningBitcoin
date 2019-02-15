using System.Numerics;

namespace BitcoinMaths
{
    public static class BigIntegerExtension
    {
        public static BigInteger Mod(this BigInteger x, BigInteger mod) => (x % mod + mod) % mod;
    }
}
