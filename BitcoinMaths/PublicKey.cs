using System;
using System.Linq;
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

        public byte[] ToUncompressedSecFormat()
        {
            return (new byte[] { Serialisation.SEC_MARKER_UNCOMPRESSED })
                .Concat(Point.X.Value.ToByteArrayUnsignedBigEndian(32))
                .Concat(Point.Y.Value.ToByteArrayUnsignedBigEndian(32))
                .ToArray();
        }

        public byte[] ToCompressedSecFormat()
        {
            var markerByte = Point.Y.Value % 2 == 0 ?
                Serialisation.SEC_MARKER_COMPRESSED_EVEN :
                Serialisation.SEC_MARKER_COMPRESSED_ODD;

            return (new byte[] { markerByte })
                .Concat(Point.X.Value.ToByteArrayUnsignedBigEndian(32))
                .ToArray();
        }

        public static PublicKey Parse(byte[] secBuffer)
        {
            var marker = secBuffer[0];

            if(marker < Serialisation.SEC_MARKER_MIN || marker > Serialisation.SEC_MARKER_MAX)
            {
                throw new ArgumentException($"Invalid marker byte. {marker:x2} supplied.");
            }

            var curve = Secp256k1.Curve;
            FiniteFieldElement x, y;

            if (marker == Serialisation.SEC_MARKER_UNCOMPRESSED)
            {
                if (secBuffer.Length != Serialisation.SEC_LENGTH_UNCOMPRESSED)
                {
                    throw new ArgumentException($"Uncompressed SEC buffer (prefix {marker:x2}) must be {Serialisation.SEC_LENGTH_UNCOMPRESSED} bytes long. {secBuffer.Length} bytes supplied.");
                }

                x = new FiniteFieldElement(new BigInteger(secBuffer.Skip(1).Take(32).Reverse().Concat(new byte[] { 0 }).ToArray()), Secp256k1.P);
                y = new FiniteFieldElement(new BigInteger(secBuffer.Skip(33).Take(32).Reverse().Concat(new byte[] { 0 }).ToArray()), Secp256k1.P);
                return new PublicKey(new EllipticCurveFiniteFieldPoint(x, y, curve));
            }


            if (secBuffer.Length != Serialisation.SEC_LENGTH_COMPRESSED)
            {
                throw new ArgumentException($"Compressed SEC buffer (prefix {marker:x2}) must be {Serialisation.SEC_LENGTH_COMPRESSED} bytes long. {secBuffer.Length} bytes supplied.");
            }
            x = new FiniteFieldElement(new BigInteger(secBuffer.Skip(1).Take(32).Reverse().Concat(new byte[] { 0 }).ToArray()), Secp256k1.P);
            var alpha = x.Pow(3) + curve.B;
            var beta = alpha.Sqrt();
            y = (marker == 2 ^ beta.Value % 2 == 0) ?
                new FiniteFieldElement(Secp256k1.P - beta.Value, Secp256k1.P) :
                beta;
            return new PublicKey(new EllipticCurveFiniteFieldPoint(x, y, curve));
        }
    }
}
