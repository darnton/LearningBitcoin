using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class ByteArrayExtensionTests
    {
        [TestMethod]
        public void Segment_result()
        {
            var buffer = new byte[] { 255, 254, 253, 252 };

            var expectedResult = new byte[] { 254, 253 };
            var actualResult = buffer.Segment(1, 2);

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void PrefixZeroes_result()
        {
            var buffer = new byte[] { 255, 254 };

            var expectedResult = new byte[] { 0, 0, 255, 254 };
            var actualResult = buffer.PrefixZeroes(2);

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void PostfixZeroes_result()
        {
            var buffer = new byte[] { 255, 254 };

            var expectedResult = new byte[] { 255, 254, 0, 0 };
            var actualResult = buffer.PostfixZeroes(2);

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        #region ToBigInteger
        [TestMethod]
        public void ToBigInteger_littleEndianSigned()
        {
            var buffer = new byte[] { 0, 128 };

            var expectedValue = -32768;
            var actualValue = buffer.ToBigInteger(ByteArrayFormat.LittleEndianSigned);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToBigInteger_bigEndianSigned()
        {
            var buffer = new byte[] { 128, 0 };

            var expectedValue = -32768;
            var actualValue = buffer.ToBigInteger(ByteArrayFormat.BigEndianSigned);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToBigInteger_littleEndianUnsigned()
        {
            var buffer = new byte[] { 0, 128 };

            var expectedValue = 32768;
            var actualValue = buffer.ToBigInteger(ByteArrayFormat.LittleEndianUnsigned);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToBigInteger_bigEndianUnsigned()
        {
            var buffer = new byte[] { 128, 0 };

            var expectedValue = 32768;
            var actualValue = buffer.ToBigInteger(ByteArrayFormat.BigEndianUnsigned);

            Assert.AreEqual(expectedValue, actualValue);
        }
        #endregion
    }
}
