using System.Numerics;
using BitcoinMaths;

namespace Bitcoin
{
    public class KeyPair
    {
        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public KeyPair(BigInteger secret)
        {
            PrivateKey = new PrivateKey(secret);
            PublicKey = new PublicKey(secret * Secp256k1.G);
        }
    }
}
