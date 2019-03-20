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
        public void Parse_Length()
        {
            var scriptHex = "6a47304402207899531a52d59a6de200179928ca900254a36b8dff8bb75f5f5d71b1cdc26125022008b422690b8461cb52c3cc30330b23d574351872b7c361e9aae3649071c1a7160121035d5c93d9ac96881f19ba1f686f15f009ded7c62efe85a872e6a19b43c15a2937";
            var reader = new BinaryReader(new MemoryStream(scriptHex.GetBytesFromHex()));

            var script = Script.Parse(reader);
            var expectedLength = 106;
            var actualLength = script.Length;

            Assert.AreEqual(expectedLength, actualLength);
        }
    }
}
