using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class FiniteFieldElementTests
    {
        #region constructor
        [TestMethod]
        public void constructor_throwsWhenValueNegative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new FiniteFieldElement(-1, 3));
        }

        [TestMethod]
        public void constructor_throwsWhenValueEqualsOrder()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new FiniteFieldElement(3, 3));
        }

        [TestMethod]
        public void constructor_throwsWhenValueGreaterThanOrder()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new FiniteFieldElement(3, 2));
        }
        #endregion

        #region equality
        [TestMethod]
        public void operatorEquals_trueWhenValueAndOrderMatch()
        {
            var a = new FiniteFieldElement(1, 7);
            var b = new FiniteFieldElement(1, 7);

            Assert.AreEqual(true, a == b);
            Assert.AreEqual(false, a != b);
        }

        [TestMethod]
        public void operatorEquals_falseWhenNull()
        {
            FiniteFieldElement a = null;
            var b = new FiniteFieldElement(1, 7);

            Assert.AreEqual(false, a == b);
            Assert.AreEqual(false, a != b);
        }

        [TestMethod]
        public void operatorNotEquals_falseWhenValuesDontMatch()
        {
            var a = new FiniteFieldElement(1, 7);
            var b = new FiniteFieldElement(2, 7);

            Assert.AreEqual(true, a != b);
            Assert.AreEqual(false, a == b);
        }
        #endregion

        #region addition
        [TestMethod]
        public void operatorAddition_throwsWhenOrdersDontMatch()
        {
            var a = new FiniteFieldElement(7, 11);
            var b = new FiniteFieldElement(12, 13);

            Assert.ThrowsException<ArgumentException>(() => a + b);
        }

        [TestMethod]
        public void operatorAddition_modulo()
        {
            var a = new FiniteFieldElement(7, 13);
            var b = new FiniteFieldElement(12, 13);
            var c = new FiniteFieldElement(6, 13);

            Assert.AreEqual(c, a + b);
        }
        #endregion

        [TestMethod]
        public void operatorSubtraction_throwsWhenOrdersDontMatch()
        {
            var a = new FiniteFieldElement(7, 11);
            var b = new FiniteFieldElement(12, 13);

            Assert.ThrowsException<ArgumentException>(() => a - b);
        }

        [TestMethod]
        public void operatorSubtraction_modulo()
        {
            var a = new FiniteFieldElement(7, 13);
            var b = new FiniteFieldElement(12, 13);
            var c = new FiniteFieldElement(8, 13);

            Assert.AreEqual(c, a - b);
        }

        [TestMethod]
        public void operatorMultiplication_throwsWhenOrdersDontMatch()
        {
            var a = new FiniteFieldElement(7, 11);
            var b = new FiniteFieldElement(12, 13);

            Assert.ThrowsException<ArgumentException>(() => a * b);
        }

        [TestMethod]
        public void operatorMulitplication_modulo()
        {
            var a = new FiniteFieldElement(7, 13);
            var b = new FiniteFieldElement(12, 13);
            var c = new FiniteFieldElement(6, 13);

            Assert.AreEqual(c, a * b);
        }

        [TestMethod]
        public void pow_modulo()
        {
            var a = new FiniteFieldElement(3, 13);
            var exponent = 3;
            var c = new FiniteFieldElement(1, 13);

            Assert.AreEqual(c, a.Pow(exponent));
        }

        [TestMethod]
        public void pow_intermediateValueInt32Overflow()
        {
            var a = new FiniteFieldElement(7, 13);
            var exponent = 12;
            var c = new FiniteFieldElement(1, 13);

            Assert.AreEqual(c, a.Pow(exponent));
        }

        [TestMethod]
        public void operatorDivision_throwsWhenOrdersDontMatch()
        {
            var a = new FiniteFieldElement(7, 11);
            var b = new FiniteFieldElement(12, 13);

            Assert.ThrowsException<ArgumentException>(() => a / b);
        }

        [TestMethod]
        public void operatorDivision_modulo()
        {
            var a = new FiniteFieldElement(2, 19);
            var b = new FiniteFieldElement(7, 19);
            var c = new FiniteFieldElement(3, 19);

            Assert.AreEqual(c, a / b);
        }
    }
}
