using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class BigIntegerExtensionTests
    {
        #region Mod
        [TestMethod]
        public void Mod_positiveDividendPositiveDivisor()
        {
            Assert.AreEqual(new BigInteger(6), (new BigInteger(19)).Mod(new BigInteger(13)));
        }

        [TestMethod]
        public void Mod_negativeDividendPositiveDivisor()
        {
            Assert.AreEqual(new BigInteger(8), (new BigInteger(-5)).Mod(new BigInteger(13)));
        }
        #endregion
    }
}
