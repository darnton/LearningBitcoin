using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;
using BitcoinMaths;

namespace BitcoinTest
{
    [TestClass]
    public class PrivateKeyTests
    {
        //Test values from Programming Bitcoin Ch 4.
        [TestMethod]
        public void ToWifFormat_testnetUncompressedResult()
        {
            var secret = BigInteger.Pow(new BigInteger(2021), 5);
            var kp = new KeyPair(secret);

            var expectedWif = "91avARGdfge8E4tZfYLoxeJ5sGBdNJQH4kvjpWAxgzczjbCwxic";
            var actualWif = kp.PrivateKey.ToWifFormat(SerialisationFormat.TestNet);

            Assert.AreEqual(expectedWif, actualWif);
        }

        [TestMethod]
        public void ToWifFormat_mainnetCompressedResult()
        {
            var secret = BigInteger.Parse("54321deadbeef", NumberStyles.AllowHexSpecifier);
            var kp = new KeyPair(secret);

            var expectedWif = "KwDiBf89QgGbjEhKnhXJuH7LrciVrZi3qYjgiuQJv1h8Ytr2S53a";
            var actualWif = kp.PrivateKey.ToWifFormat(SerialisationFormat.Compressed);

            Assert.AreEqual(expectedWif, actualWif);
        }
    }
}
