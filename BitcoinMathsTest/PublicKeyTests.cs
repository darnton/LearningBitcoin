using System.Globalization;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class PublicKeyTests
    {
        [TestMethod]
        public void Verify_trueResult()
        {
            var r = BigInteger.Parse("0037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c6", NumberStyles.AllowHexSpecifier);
            var s = BigInteger.Parse("008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec", NumberStyles.AllowHexSpecifier);
            var sig = new Signature(r, s);

            var z = BigInteger.Parse("00bc62d4b80d9e36da29c16c5d4d9f11731f36052c72401a76c23c0fb5a9b74423", NumberStyles.AllowHexSpecifier);
            var x = BigInteger.Parse("0004519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574", NumberStyles.AllowHexSpecifier);
            var y = BigInteger.Parse("0082b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4", NumberStyles.AllowHexSpecifier);
            var pk = new PublicKey(Secp256k1.Curve.GetPoint(x, y));

            Assert.IsTrue(pk.Verify(sig, z));
        }

        [TestMethod]
        public void Verify_falseBadHash()
        {
            var r = BigInteger.Parse("0037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c6", NumberStyles.AllowHexSpecifier);
            var s = BigInteger.Parse("008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec", NumberStyles.AllowHexSpecifier);
            var sig = new Signature(r, s);

            var z = BigInteger.Parse("00ffffffff0d9e36da29c16c5d4d9f11731f36052c72401a76c23c0fb5a9b74423", NumberStyles.AllowHexSpecifier);
            var x = BigInteger.Parse("0004519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574", NumberStyles.AllowHexSpecifier);
            var y = BigInteger.Parse("0082b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4", NumberStyles.AllowHexSpecifier);
            var pk = new PublicKey(Secp256k1.Curve.GetPoint(x, y));

            Assert.IsFalse(pk.Verify(sig, z));
        }
    }
}
