using System.Linq;
using System.Numerics;
using BitcoinMaths;

namespace Bitcoin
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
            S = ((documentHash + R * kp.PrivateKey.Secret) * kInv).Mod(Secp256k1.N);
            //S = s > Secp256k1.N / 2 ? Secp256k1.N - s : s; //Low-s malleability
        }

        public byte[] ToDerFormat()
        {
            var rBytes = R.ToByteArray(ByteArrayFormat.BigEndianUnsigned);
            if (rBytes[0] > 127)
            {
                rBytes = rBytes.PrefixZeroes(1);
            }
            var sBytes = S.ToByteArray(ByteArrayFormat.BigEndianUnsigned);
            if (sBytes[0] > 127)
            {
                sBytes = sBytes.PrefixZeroes(1);
            }

            var totalLength = rBytes.Length + sBytes.Length + 6;
            var derBuffer = new byte[totalLength];
            derBuffer[0] = Serialisation.DER_START_MARKER;
            derBuffer[1] = (byte)(totalLength - 2);
            derBuffer[2] = Serialisation.DER_INTEGER_MARKER;
            derBuffer[3] = (byte)rBytes.Length;
            rBytes.CopyTo(derBuffer, 4);
            derBuffer[rBytes.Length + 4] = Serialisation.DER_INTEGER_MARKER;
            derBuffer[rBytes.Length + 5] = (byte)sBytes.Length;
            sBytes.CopyTo(derBuffer, rBytes.Length + 6);

            return derBuffer;
        }

        public override string ToString()
        {
            return $"Signature({R}, {S})";
        }

        public static Signature Parse(byte[] derBuffer)
        {
            var totalLength = derBuffer[1];
            if (derBuffer.Length != totalLength + 2)
            {
                throw new ParsingException($"Failed to parse Signature DER buffer. Expected {totalLength} bytes. Buffer contained {derBuffer.Length - 1}.");
            }
            var rLength = derBuffer[3];
            var sLength = derBuffer[5 + rLength];
            var rBytes = derBuffer.Skip(4).Take(rLength).ToArray();
            var r = rBytes.ToBigInteger(ByteArrayFormat.BigEndianUnsigned);
            var sBytes = derBuffer.Skip(6 + rLength).Take(sLength).ToArray();
            var s = sBytes.ToBigInteger(ByteArrayFormat.BigEndianUnsigned);

            return new Signature(r, s);
        }
    }
}
