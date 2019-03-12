using System;
using System.Numerics;

namespace BitcoinMaths
{
    public static class EphemeralPrivateKey_INSECURE
    {
        private const int size = 32;

        public static BigInteger New()
        {
            var k = BigInteger.Zero;
            while(k <= 0 || k >= Secp256k1.P)
            {
                var rnd = new Random();
                var randomBytes = new byte[size + 1];
                rnd.NextBytes(randomBytes);
                randomBytes[size] = 0; //BigInteger parses most significant bit as sign.
                k = new BigInteger(randomBytes);
            }
            return k;
        }
    }
}
