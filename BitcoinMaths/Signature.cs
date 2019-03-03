using System.Numerics;

namespace BitcoinMaths
{
    public class Signature
    {
        public BigInteger R { get; }
        public BigInteger S { get; }

        public Signature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }

        public Signature(KeyPair kp, BigInteger documentHash, BigInteger ephemeralPrivateKey)
        {
            R = (ephemeralPrivateKey * Secp256k1.G).X.Value;
            var kInv = BigInteger.ModPow(ephemeralPrivateKey, Secp256k1.N - 2, Secp256k1.N); //Fermat's Little Theorem
            S = ((documentHash + R * kp.PrivateKey) * kInv).Mod(Secp256k1.N);
            //S = s > Secp256k1.N / 2 ? Secp256k1.N - s : s; //Low-s malleability
        }

        public override string ToString()
        {
            return $"Signature({R}, {S})";
        }
    }
}
