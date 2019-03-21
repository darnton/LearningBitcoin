using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;
using BitcoinMaths;

namespace BitcoinTest
{
    [TestClass]
    public class ScriptTests
    {
        [TestMethod]
        public void Parse_outputCommandText()
        {
            var scriptHex = "1976a914ab0c0b2e98b1ab6dbf67d4750b0a56244948a87988ac";
            var reader = new BinaryReader(new MemoryStream(scriptHex.GetBytesFromHex()));

            var script = Script.Parse(reader);
            var expectedText = "OP_DUP OP_HASH160 ab0c0b2e98b1ab6dbf67d4750b0a56244948a879 OP_EQUALVERIFY OP_CHECKSIG";
            var actualText = script.ToString();

            Assert.AreEqual(expectedText, actualText);
        }

        [TestMethod]
        public void Serialise_roundTrip()
        {
            var scriptHex = "1976a914ab0c0b2e98b1ab6dbf67d4750b0a56244948a87988ac";
            var reader = new BinaryReader(new MemoryStream(scriptHex.GetBytesFromHex()));
            var script = Script.Parse(reader);

            var actualBytes = new byte[26];
            script.Serialise(new BinaryWriter(new MemoryStream(actualBytes)));

            var actualText = actualBytes.EncodeAsHex();

            Assert.AreEqual(scriptHex, actualText);
        }
    }
}
