using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class SignatureTests
    {
        #region constructor
        [TestMethod]
        public void constructor_createFromKeyAndHash()
        {
            //Values from Programming Bitcoin Ch 3.
            //"Brain wallet". Don't do this.
            var secret = Hash.DoubleSha256("my secret").ToBigInteger(ByteArrayFormat.BigEndianUnsigned);
            var kp = new KeyPair(secret);
            
            var z = Hash.DoubleSha256("my message").ToBigInteger(ByteArrayFormat.BigEndianUnsigned);
            
            var k = new BigInteger(1234567890); //Fixed to get a known result
            var r = BigInteger.Parse("002b698a0f0a4041b77e63488ad48c23e8e8838dd1fb7520408b121697b782ef22", NumberStyles.AllowHexSpecifier);
            var s = BigInteger.Parse("00bb14e602ef9e3f872e25fad328466b34e6734b7a0fcd58b1eb635447ffae8cb9", NumberStyles.AllowHexSpecifier);

            var sig = new Signature(kp, z, k);

            Assert.AreEqual(r, sig.R);
            Assert.AreEqual(s, sig.S);
        }
        #endregion

        [TestMethod]
        public void ToDerFormat_result()
        {
            var r = BigInteger.Parse("002b698a0f0a4041b77e63488ad48c23e8e8838dd1fb7520408b121697b782ef22", NumberStyles.AllowHexSpecifier);
            var s = BigInteger.Parse("00bb14e602ef9e3f872e25fad328466b34e6734b7a0fcd58b1eb635447ffae8cb9", NumberStyles.AllowHexSpecifier);
            var sig = new Signature(r, s);

            var expectedDerBytes = Serialisation.GetBytesFromHex("304502202b698a0f0a4041b77e63488ad48c23e8e8838dd1fb7520408b121697b782ef22022100bb14e602ef9e3f872e25fad328466b34e6734b7a0fcd58b1eb635447ffae8cb9");
            var actualDerBytes = sig.ToDerFormat();

            CollectionAssert.AreEqual(expectedDerBytes, actualDerBytes);
        }
    }
}
