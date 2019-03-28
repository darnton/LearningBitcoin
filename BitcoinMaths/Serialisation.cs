using System;
using System.Linq;
using System.Text;

namespace BitcoinMaths
{
    //TODO: Find all references and use extension methods
    public static class Serialisation
    {
        public const byte ADDRESS_MARKER_TESTNET = 111;
        public const byte ADDRESS_MARKER_MAINNET = 0;

        public const string BASE58_ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        public const byte DER_START_MARKER = 48;
        public const byte DER_INTEGER_MARKER = 2;

        public const byte SEC_MARKER_MIN = 2;
        public const byte SEC_MARKER_MAX = 4;
        public const byte SEC_MARKER_COMPRESSED_EVEN = 2;
        public const byte SEC_MARKER_COMPRESSED_ODD = 3;
        public const byte SEC_MARKER_UNCOMPRESSED = 4;

        public const int SEC_LENGTH_UNCOMPRESSED = 65;
        public const int SEC_LENGTH_COMPRESSED = 33;

        public static byte[] GetBytesFromHex(this string hex)
        {
            if (hex.Length % 2 != 0)
            {
                hex = "0" + hex;
            }
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }

        public static string EncodeAsHex(this byte[] buffer)
        {
            return string.Join("", buffer.Select(b => b.ToString("x2")));
        }

        public static string EncodeAsBase58(this byte[] buffer)
        {
            var base58 = new StringBuilder();
            var leadingZeroesCount = buffer.TakeWhile(b => b == 0).Count();
            base58.Append('1', leadingZeroesCount);

            var number = buffer.ToBigInteger(ByteArrayFormat.BigEndianUnsigned);
            var base58CharBuffer = new StringBuilder(); //Least significant character first
            while (number > 0)
            {
                base58CharBuffer.Append(BASE58_ALPHABET[(int)number.Mod(58)]);
                number = number / 58;
            }

            base58.Append(base58CharBuffer.ToString().Reverse().ToArray());
            return base58.ToString();
        }

        public static string EncodeAsBase58Check(this byte[] addressBytes, Base58CheckType type)
        {
            var prefix = GetBase58CheckPrefix(type);
            var prefixedLength = prefix.Length + addressBytes.Length;
            var addressBuffer = new byte[prefixedLength + 4];

            prefix.CopyTo(addressBuffer, 0);
            addressBytes.CopyTo(addressBuffer, prefix.Length);

            var checkBytes = Hash.DoubleSha256(addressBuffer.Segment(0, prefixedLength)).Segment(0, 4);
            checkBytes.CopyTo(addressBuffer, prefixedLength);

            return EncodeAsBase58(addressBuffer);
        }

        private static byte[] GetBase58CheckPrefix(Base58CheckType type)
        {
            switch (type)
            {
                case Base58CheckType.MAINNET_ADDRESS:
                    return new byte[] { 0 };
                case Base58CheckType.P2SH_ADDRESS:
                    return new byte[] { 5 };
                case Base58CheckType.TESTNET_ADDRESS:
                    return new byte[] { 111 };
                case Base58CheckType.PRIVATE_KEY_WIF:
                    return new byte[] { 128 };
                case Base58CheckType.PRIVATE_KEY_WIF_TESTNET:
                    return new byte[] { 239 };
                case Base58CheckType.BIP38_PRIVATE_KEY:
                    return new byte[] { 1, 66 };
                case Base58CheckType.BIP32_PUBLIC_KEY:
                    return new byte[] { 4, 136, 178, 30 };
                default:
                    throw new ArgumentOutOfRangeException($"Invalid Base58CheckType ({type}).");
            }
        }
    }
}
