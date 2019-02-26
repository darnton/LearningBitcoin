using System.Numerics;

namespace BitcoinMaths
{
    public class KeyPair
    {
        internal BigInteger PrivateKey { get; }
        public EllipticCurveFiniteFieldPoint PublicKey { get; }

        public KeyPair(BigInteger secret)
        {
            PrivateKey = secret;
            PublicKey = PrivateKey * Secp256k1.G;
        }
    }
}
