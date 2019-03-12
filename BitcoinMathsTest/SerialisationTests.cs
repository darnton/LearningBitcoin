using System.Linq;
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
        public void GetHexFromBytes_result()
        {
            var buffer = new byte[] { 255, 254 };

            var expectedHex = "fffe";
            var actualHex = Serialisation.GetHexFromBytes(buffer);

            Assert.AreEqual(expectedHex, actualHex);
        }
    }
}
