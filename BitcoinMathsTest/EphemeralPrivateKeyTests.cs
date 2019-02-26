using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class EphemeralPrivateKeyTests
    {
        [TestMethod]
        public void New_KeyInRange()
        {
            var k = EphemeralPrivateKey_INSECURE.New();

            Assert.IsTrue(k > 0);
            Assert.IsTrue(k < Secp256k1.P);
        }
    }
}
