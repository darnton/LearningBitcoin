using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class ByteArrayExtensionTests
    {
        #region ToUnsignedBigInteger
        [TestMethod]
        public void ToUnsignedBigInteger_mostSignificantBitSet()
        {
            //Big-endian array
            var buffer = new byte[] { 255, 254 };

            var result = buffer.ToUnsignedBigInteger();

            Assert.AreEqual(65534, result);
        }

        [TestMethod]
        public void ToUnsignedBigInteger_bigEndianParam()
        {
            //Big-endian array
            var documentHash = Hash.DoubleSha256("my message").ToUnsignedBigInteger();
            var z = BigInteger.Parse("000231c6f3d980a6b0fb7152f85cee7eb52bf92433d9919b9c5218cb08e79cce78", NumberStyles.AllowHexSpecifier);

            Assert.AreEqual(z, documentHash);
        }
        #endregion
    }
}
