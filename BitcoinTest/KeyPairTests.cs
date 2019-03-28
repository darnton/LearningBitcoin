using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;
using BitcoinMaths;

namespace BitcoinTest
{
    [TestClass]
    public class KeyPairTests
    {
        #region constructor
        [TestMethod]
        public void constructor_createFromSecret()
        {
            //Values from Mastering Bitcoin, Ch 4.
            var secret = BigInteger.Parse("001E99423A4ED27608A15A2616A2B0E9E52CED330AC530EDCC32C8FFC6A526AEDD", NumberStyles.AllowHexSpecifier);
            var expectedPublicKeyPoint = new EllipticCurveFiniteFieldPoint(
                BigInteger.Parse("00F028892BAD7ED57D2FB57BF33081D5CFCF6F9ED3D3D7F159C2E2FFF579DC341A", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0007CF33DA18BD734C600B96A72BBC4749D5141C90EC8AC328AE52DDFE2E505BDB", NumberStyles.AllowHexSpecifier),
                Secp256k1.Curve);

            var actualPublicKey = new KeyPair(secret).PublicKey;

            Assert.AreEqual(expectedPublicKeyPoint, actualPublicKey.Point);
        }
        #endregion
    }
}
