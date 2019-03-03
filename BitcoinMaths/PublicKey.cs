using System.Numerics;

namespace BitcoinMaths
{
    public class PublicKey
    {
        public EllipticCurveFiniteFieldPoint Point { get; }

        public PublicKey(EllipticCurveFiniteFieldPoint point)
        {
            Point = point;
        }

        public bool Verify(Signature sig, BigInteger documentHash)
        {
            var sInv = BigInteger.ModPow(sig.S, Secp256k1.N - 2, Secp256k1.N); //Fermat's Little Theorem
            var u = (documentHash * sInv).Mod(Secp256k1.N);
            var v = (sig.R * sInv).Mod(Secp256k1.N);
            var total = u * Secp256k1.G + v * Point;
            return total.X.Value == sig.R;
        }
    }
}
