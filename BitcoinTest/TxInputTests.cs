using System.IO;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;
using BitcoinMaths;

namespace BitcoinTest
{
    [TestClass]
    public class TxInputTests
    {
        [TestMethod]
        public void Parse_fieldsParsed()
        {
            var txIdHex = "56919960ac691763688d3d3bcea9ad6ecaf875df5339e148a1fc61c6ed7a069e";
            var inputHex = txIdHex +
                "01000000" +
                "6a" + //script length 106
                    "47304402204585bcdef85e6b1c6af5c2669d4830ff86e42dd205c0e089bc2a821657e951c002201024a10366077f87d6bce1f7100ad8cfa8a064b39d4e8fe4ea13a7b71aa8180f012102f0da57e85eec2934a82a585ea337ce2f4998b50ae699dd79f5880e253dafafb7" +
                "feffffff";
            var reader = new BinaryReader(new MemoryStream(inputHex.GetBytesFromHex()));

            var input = TxInput.Parse(reader);
            var expectedTxId = new BigInteger((txIdHex + "00").GetBytesFromHex());
            var actualTxId = input.PreviousTxId;
            var expectedTxIndex = (uint)1;
            var actualTxIndex = input.PreviousTxIndex;
            var expectedSequence = uint.MaxValue - 1;
            var actualSequence = input.Sequence;

            Assert.AreEqual(expectedTxId, actualTxId);
            Assert.AreEqual(expectedTxIndex, actualTxIndex);
            Assert.AreEqual(expectedSequence, actualSequence);
        }
    }
}
