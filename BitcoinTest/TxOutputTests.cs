using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;
using BitcoinMaths;

namespace BitcoinTest
{
    [TestClass]
    public class TxOutputTests
    {
        [TestMethod]
        public void Parse_fieldsParsed()
        {
            var outputHex = "51430f0000000000" + // amount
                "19" + // script length 25
                    "76a914ab0c0b2e98b1ab6dbf67d4750b0a56244948a87988ac";
            var reader = new BinaryReader(new MemoryStream(outputHex.GetBytesFromHex()));

            var output = TxOutput.Parse(reader);
            var expectedAmount = (ulong)1000273;
            var actualAmount = output.Amount;

            Assert.AreEqual(expectedAmount, actualAmount);
        }
    }
}
