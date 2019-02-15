using System.Globalization;
using System.Numerics;

namespace BitcoinMaths
{
    /// <summary>
    /// secp256k1 curve parameters
    /// from https://en.bitcoin.it/wiki/Secp256k1
    /// </summary>
    public static class Secp256k1
    {
        // Highest order bit parsed as sign, so lead with 00.
        // https://docs.microsoft.com/en-us/dotnet/api/system.numerics.biginteger.parse
        public static BigInteger P { get; } = BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", NumberStyles.AllowHexSpecifier);

        public static BigInteger A = BigInteger.Zero;

        public static BigInteger B = new BigInteger(7);

        public static BigInteger Gx { get; } = BigInteger.Parse("0079BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", NumberStyles.AllowHexSpecifier);

        public static BigInteger Gy { get; } = BigInteger.Parse("00483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", NumberStyles.AllowHexSpecifier);

        public static BigInteger N { get; } = BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.AllowHexSpecifier);
    
        public static EllipticCurveOverFiniteField Curve { get; } = new EllipticCurveOverFiniteField(A, B, P);

        public static EllipticCurveFiniteFieldPoint G = Curve.GetPoint(Gx, Gy);
    }
}
