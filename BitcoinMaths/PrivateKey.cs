using System.Linq;
using System.Numerics;

namespace BitcoinMaths
{
    public class PrivateKey
    {
        internal BigInteger Secret { get; }

        public PrivateKey(BigInteger secret)
        {
            Secret = secret;
        }

        public string ToWifFormat(SerialisationFormat format)
        {
            var type = (format & SerialisationFormat.TestNet) > 0 ?
                Base58CheckType.PRIVATE_KEY_WIF_TESTNET : Base58CheckType.PRIVATE_KEY_WIF;
            var wifBuffer = Secret.ToByteArray(ByteArrayFormat.BigEndianUnsigned, 32);
            if ((format & SerialisationFormat.Compressed) > 0)
            {
                wifBuffer = wifBuffer.Concat(new byte[] { 1 }).ToArray();
            }
            return Serialisation.EncodeAsBase58Check(wifBuffer, type);
        }
    }
}
