using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class EllipticCurveOverFiniteFieldTests
    {
        [TestMethod]
        public void PointIsOnCurve_testPoints()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);

            Assert.IsTrue(testCurve.PointIsOnCurve(192, 105));
            Assert.IsTrue(testCurve.PointIsOnCurve(17, 56));
            Assert.IsTrue(testCurve.PointIsOnCurve(1, 193));
            Assert.IsFalse(testCurve.PointIsOnCurve(200, 119));
            Assert.IsFalse(testCurve.PointIsOnCurve(42, 99));
        }
    }
}
