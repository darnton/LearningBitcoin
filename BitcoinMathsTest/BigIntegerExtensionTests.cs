using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class BigIntegerExtensionTests
    {
        #region Mod
        [TestMethod]
        public void Mod_positiveDividendPositiveDivisor()
        {
            Assert.AreEqual(new BigInteger(6), (new BigInteger(19)).Mod(new BigInteger(13)));
        }

        [TestMethod]
        public void Mod_negativeDividendPositiveDivisor()
        {
            Assert.AreEqual(new BigInteger(8), (new BigInteger(-5)).Mod(new BigInteger(13)));
        }
        #endregion

        #region ToByteArrayBigEndian
        [TestMethod]
        public void ToByteArrayBigEndian_mostSignificantByteFirst()
        {
            var x = BigInteger.Parse("0082b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4", NumberStyles.AllowHexSpecifier);
            byte expectedFirstByte = 130; //0x82

            Assert.AreEqual(expectedFirstByte, x.ToByteArrayUnsignedBigEndian(32)[0]);
        }

        [TestMethod]
        public void ToByteArrayBigEndian_correctLength()
        {
            var x = BigInteger.Parse("00ffff", NumberStyles.AllowHexSpecifier);
            var expectedLength = 32;

            Assert.AreEqual(expectedLength, x.ToByteArrayUnsignedBigEndian(expectedLength).Length);
        }
        #endregion
    }
}
