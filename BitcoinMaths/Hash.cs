using System.Security.Cryptography;
using System.Text;

namespace BitcoinMaths
{
    public static class Hash
    {
        public static byte[] Sha256(this byte[] buffer)
        {
            var sha256 = SHA256.Create();
            return sha256.ComputeHash(buffer);
        }

        /// <summary>
        /// Performs a double SHA-256 hash as required by Bitcoin
        /// to defend against length-extension attack.
        /// </summary>
        /// <param name="buffer">The byte array to be hashed.</param>
        /// <returns>The 32-byte hash</returns>
        public static byte[] DoubleSha256(this byte[] buffer)
        {
            var sha256 = SHA256.Create();
            var round1 = sha256.ComputeHash(buffer);
            return sha256.ComputeHash(round1);
        }

        public static byte[] DoubleSha256(this string message)
        {
            return DoubleSha256(Encoding.UTF8.GetBytes(message));
        }

        public static byte[] Hash160(this byte[] buffer)
        {
            var sha256 = SHA256.Create();
            var ripemd160 = RIPEMD160.Create();
            return ripemd160.ComputeHash(sha256.ComputeHash(buffer));
        }
    }
}
