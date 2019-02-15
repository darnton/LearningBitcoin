using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class EllipticCurveFiniteFieldPointTests
    {
        #region constructor
        [TestMethod]
        public void constructor_throwsWhenPointNotOnCurve()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);

            Assert.ThrowsException<ArgumentException>(() => testCurve.GetPoint(0, 105));
        }

        [TestMethod]
        public void constructor_doesntThrowWhenPointAtInfinity()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            Assert.IsNotNull(testCurve.GetPoint(192, null));
        }
        #endregion

        [TestMethod]
        public void AtInfinity_trueWhenYNull()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(192, null);

            Assert.IsTrue(a.AtInfinity);
        }

        [TestMethod]
        public void SlopeOfTangent_testResult()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(47, 71);
            var s = new FiniteFieldElement(199, testCurve.Order);

            Assert.AreEqual(s, a.SlopeOfTangent);
        }

        #region equality
        [TestMethod]
        public void operatorEquals_trueWhenXAndYMatch()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(192, 105);
            var b = testCurve.GetPoint(192, 105);

            Assert.AreEqual(true, a == b);
            Assert.AreEqual(false, a != b);
        }

        [TestMethod]
        public void operatorNotEquals_falseWhenValuesDontMatch()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(192, 105);
            var b = testCurve.GetPoint(17, 56);

            Assert.AreEqual(true, a != b);
            Assert.AreEqual(false, a == b);
        }
        #endregion

        #region addition
        [TestMethod]
        public void operatorAddition_throwsWhenCurvesDontMatch()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(170, 142);
            var altCurve = new EllipticCurveOverFiniteField(0, 1, 223);
            var b = altCurve.GetPoint(2, 3);

            Assert.ThrowsException<ArgumentException>(() => a + b);
        }

        [TestMethod]
        public void operatorAddition_pointAtInfinityReturnsOther()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(192, 105);
            var b = testCurve.GetPoint(192, null);

            Assert.AreEqual(a, a + b);
        }

        [TestMethod]
        public void operatorAddition_pointsAtSameXReturnInfinity()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(192, 105);
            var b = testCurve.GetPoint(192, 118); // -105 maps to 118 in F(223)
            var c = testCurve.GetPoint(192, null);

            Assert.AreEqual(c, a + b);
        }

        [TestMethod]
        public void operatorAddition_twoPointsReturnTheThird()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(170, 142);
            var b = testCurve.GetPoint(60, 139);
            var c = testCurve.GetPoint(220, 181);

            Assert.AreEqual(c, a + b);
        }

        //[TestMethod]
        //public void operatorAddition_tangentLinesReturnIntersection()
        //{
        //    var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
        //    var a = new EllipticCurveFiniteFieldPoint(-1, 1, 5, 7);
        //    var b = new EllipticCurveFiniteFieldPoint(-1, 1, 5, 7);
        //    var c = new EllipticCurveFiniteFieldPoint(18, -77, 5, 7);

        //    Assert.AreEqual(c, a + b);
        //}
        #endregion

        #region multiplication
        [TestMethod]
        public void operatorMultiplication_variousScalars()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(47, 71);
            var result2 = testCurve.GetPoint(36, 111);
            var result3 = testCurve.GetPoint(15, 137);
            var result4 = testCurve.GetPoint(194, 51);
            var result5 = testCurve.GetPoint(126, 96);
            var result10 = testCurve.GetPoint(154, 150);
            var result20 = testCurve.GetPoint(47, 152);

            Assert.AreEqual(result2, 2 * a);
            Assert.AreEqual(result3, 3 * a);
            Assert.AreEqual(result4, 4 * a);
            Assert.AreEqual(result5, 5 * a);
            Assert.AreEqual(result10, 10 * a);
            Assert.AreEqual(result20, 20 * a);
        }

        [TestMethod]
        public void operatorMultiplication_resultAtInfinity()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(47, 71);

            Assert.IsTrue((21 * a).AtInfinity);
        }

        [TestMethod]
        public void operatorMultiplication_resultsWrap()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(47, 71);

            Assert.AreEqual(2 * a, 23 * a);
        }

        [TestMethod]
        public void operatorMultiplication_commutative()
        {
            var testCurve = new EllipticCurveOverFiniteField(0, 7, 223);
            var a = testCurve.GetPoint(47, 71);

            Assert.AreEqual(a, 1 * a);
            Assert.AreEqual(a, a * 1);
        }
        #endregion
    }
}
