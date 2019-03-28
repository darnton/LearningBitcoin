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

        [TestMethod]
        public void operatorAddition_executeCombined()
        {
            var pubKeyHex = "444c4104887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c61de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34ac";
            var pubKeyReader = new BinaryReader(new MemoryStream(pubKeyHex.GetBytesFromHex()));
            var pubKeyScript = Script.Parse(pubKeyReader);

            var sigHex = "483045022000eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c022100c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab601";
            var sigReader = new BinaryReader(new MemoryStream(sigHex.GetBytesFromHex()));
            var sigScript = Script.Parse(sigReader);

            var combinedScript = sigScript + pubKeyScript;

            Assert.AreEqual(5, combinedScript.Commands.Count);
        }
    }
}
