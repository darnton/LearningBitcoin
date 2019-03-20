using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class BinaryWriterExtensionTests
    {
        [TestMethod]
        public void ReadVarInt_oneByte()
        {
            var actualBytes = new byte[1];
            var stream = new MemoryStream(actualBytes);
            var writer = new BinaryWriter(stream);

            writer.WriteVarInt(250);

            var expectedBytes = new byte[] { 250 };

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void ReadVarInt_threeBytes()
        {
            var actualBytes = new byte[3];
            var stream = new MemoryStream(actualBytes);
            var writer = new BinaryWriter(stream);

            writer.WriteVarInt(555);

            var expectedBytes = new byte[] { 253, 43, 2 };

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void ReadVarInt_fiveBytes()
        {
            var actualBytes = new byte[5];
            var stream = new MemoryStream(actualBytes);
            var writer = new BinaryWriter(stream);

            writer.WriteVarInt(70015);

            var expectedBytes = new byte[] { 254, 127, 17, 1, 0 };

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [TestMethod]
        public void ReadVarInt_nineBytes()
        {
            var actualBytes = new byte[9];
            var stream = new MemoryStream(actualBytes);
            var writer = new BinaryWriter(stream);

            writer.WriteVarInt(18005558675309);

            var expectedBytes = new byte[] { 255, 109, 199, 237, 62, 96, 16, 0, 0 };

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }
    }
}
