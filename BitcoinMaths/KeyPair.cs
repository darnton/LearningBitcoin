using System.Numerics;

namespace BitcoinMaths
{
    public class KeyPair
    {
        internal BigInteger PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public KeyPair(BigInteger secret)
        {
            PrivateKey = secret;
            PublicKey = new PublicKey(PrivateKey * Secp256k1.G);
        }
    }
}
