﻿using System.Security.Cryptography;
using System.Text;

namespace BitcoinMaths
{
    public static class Hash
    {
        /// <summary>
        /// Performs a double SHA-256 hash as required by Bitcoin
        /// to defend against length-extension attack.
        /// </summary>
        /// <param name="buffer">The byte array to be hashed.</param>
        /// <returns>The 32-byte hash</returns>
        public static byte[] DoubleSha256(byte[] buffer)
        {
            var sha256 = SHA256.Create();
            var round1 = sha256.ComputeHash(buffer);
            return sha256.ComputeHash(round1);
        }

        public static byte[] DoubleSha256(string message)
        {
            return DoubleSha256(Encoding.UTF8.GetBytes(message));
        }
    }
}