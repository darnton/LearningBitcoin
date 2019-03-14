using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class SerialisationTests
    {
        [TestMethod]
        public void GetBytesFromHex_result()
        {
            var hex = "fffe";

            var expectedBytes = new byte[] { 255, 254 };
            var actualBytes = Serialisation.GetBytesFromHex(hex);

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void EncodeAsHex_result()
        {
            var buffer = new byte[] { 255, 254 };

            var expectedHex = "fffe";
            var actualHex = Serialisation.EncodeAsHex(buffer);

            Assert.AreEqual(expectedHex, actualHex);
        }

        [TestMethod]
        public void EncodeAsBase58_result()
        {
            var hex = "7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d";
            var buffer = Serialisation.GetBytesFromHex(hex);

            var expectedBase58 = "9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM6";
            var actualBase58 = Serialisation.EncodeAsBase58(buffer);

            Assert.AreEqual(expectedBase58, actualBase58);
        }
    }
}
