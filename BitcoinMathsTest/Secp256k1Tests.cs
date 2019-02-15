using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class Secp256k1Tests
    {
        [TestMethod]
        public void P_BigIntParses()
        {
            Assert.IsTrue(Secp256k1.P > 0);
        }

        [TestMethod]
        public void Curve_GeneratorPointIsOnCurve()
        {
            Assert.IsTrue(Secp256k1.Curve.PointIsOnCurve(Secp256k1.Gx, Secp256k1.Gy));
        }

        [TestMethod]
        public void GeneratorPointHasOrderN()
        {
            Assert.IsTrue((Secp256k1.N * Secp256k1.G).AtInfinity);
        }
    }
}
