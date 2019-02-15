using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class EllipticCurvePointTests
    {
        #region constructor
        [TestMethod]
        public void constructor_throwsWhenPointNotOnCurve()
        {
            Assert.ThrowsException<ArgumentException>(() => new EllipticCurvePoint(-1, -2, 5, 7));
        }

        [TestMethod]
        public void constructor_doesntThrowWhenPointAtInfinity()
        {
            Assert.IsNotNull(new EllipticCurvePoint(-1, null, 5, 7));
        }
        #endregion

        #region equality
        [TestMethod]
        public void operatorEquals_trueWhenXAndYMatch()
        {
            var a = new EllipticCurvePoint(-1, -1, 5, 7);
            var b = new EllipticCurvePoint(-1, -1, 5, 7);

            Assert.AreEqual(true, a == b);
            Assert.AreEqual(false, a != b);
        }

        [TestMethod]
        public void operatorNotEquals_falseWhenValuesDontMatch()
        {
            var a = new EllipticCurvePoint(-1, -1, 5, 7);
            var b = new EllipticCurvePoint(18, 77, 5, 7);

            Assert.AreEqual(true, a != b);
            Assert.AreEqual(false, a == b);
        }
        #endregion

        #region addition
        [TestMethod]
        public void operatorAddition_throwsWhenCurvesDontMatch()
        {
            var a = new EllipticCurvePoint(-1, -1, 5, 7);
            var b = new EllipticCurvePoint(2, 3, 0, 1);

            Assert.ThrowsException<ArgumentException>(() => a + b);
        }

        [TestMethod]
        public void operatorAddition_pointAtInfinityReturnsOther()
        {
            var a = new EllipticCurvePoint(-1, -1, 5, 7);
            var b = new EllipticCurvePoint(-1, null, 5, 7);

            Assert.AreEqual(a, a + b);
        }

        [TestMethod]
        public void operatorAddition_pointsAtSameXReturnInfinity()
        {
            var a = new EllipticCurvePoint(-1, -1, 5, 7);
            var b = new EllipticCurvePoint(-1, 1, 5, 7);
            var c = new EllipticCurvePoint(-1, null, 5, 7);

            Assert.AreEqual(c, a + b);
        }

        [TestMethod]
        public void operatorAddition_twoPointsReturnTheThird()
        {
            var a = new EllipticCurvePoint(2, 5, 5, 7);
            var b = new EllipticCurvePoint(-1, -1, 5, 7);
            var c = new EllipticCurvePoint(3, -7, 5, 7);

            Assert.AreEqual(c, a + b);
        }

        [TestMethod]
        public void operatorAddition_tangentLinesReturnIntersection()
        {
            var a = new EllipticCurvePoint(-1, 1, 5, 7);
            var b = new EllipticCurvePoint(-1, 1, 5, 7);
            var c = new EllipticCurvePoint(18, -77, 5, 7);

            Assert.AreEqual(c, a + b);
        }
        #endregion

        [TestMethod]
        public void PointIsOnCurve_testPoints()
        {
            Assert.IsTrue(EllipticCurvePoint.PointIsOnCurve(-1, -1, 5, 7));
            Assert.IsFalse(EllipticCurvePoint.PointIsOnCurve(-1, -2, 5, 7));
        }
    }
}
