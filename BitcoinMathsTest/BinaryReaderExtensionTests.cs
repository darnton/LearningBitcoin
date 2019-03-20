using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class BinaryReaderExtensionTests
    {
        [TestMethod]
        public void ReadVarInt_oneByte()
        {
            var buffer = new byte[] { 250 };
            var reader = new BinaryReader(new MemoryStream(buffer));

            ulong expectedValue = 250;
            var actualValue = reader.ReadVarInt();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ReadVarInt_threeBytes()
        {
            var buffer = new byte[] { 253, 43, 2 };
            var reader = new BinaryReader(new MemoryStream(buffer));

            ulong expectedValue = 555;
            var actualValue = reader.ReadVarInt();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ReadVarInt_fiveBytes()
        {
            var buffer = new byte[] { 254, 127, 17, 1, 0 };
            var reader = new BinaryReader(new MemoryStream(buffer));

            ulong expectedValue = 70015;
            var actualValue = reader.ReadVarInt();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ReadVarInt_nineBytes()
        {
            var buffer = new byte[] { 255, 109, 199, 237, 62, 96, 16, 0, 0 };
            var reader = new BinaryReader(new MemoryStream(buffer));

            ulong expectedValue = 18005558675309;
            var actualValue = reader.ReadVarInt();

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
