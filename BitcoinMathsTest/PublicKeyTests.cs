using System;
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

        [TestMethod]
        public void ToUncompressedSecFormat_result()
        {
            var x = BigInteger.Parse("0004519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574", NumberStyles.AllowHexSpecifier);
            var y = BigInteger.Parse("0082b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4", NumberStyles.AllowHexSpecifier);
            var pk = new PublicKey(Secp256k1.Curve.GetPoint(x, y));

            var expectedSecString = "0404519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a757482b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4";

            Assert.AreEqual(expectedSecString, Serialisation.GetHexFromBytes(pk.ToUncompressedSecFormat()));
        }

        [TestMethod]
        public void ToCompressedSecFormat_resultEven()
        {
            var x = BigInteger.Parse("0004519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574", NumberStyles.AllowHexSpecifier);
            var y = BigInteger.Parse("0082b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4", NumberStyles.AllowHexSpecifier);
            var pk = new PublicKey(Secp256k1.Curve.GetPoint(x, y));

            var expectedSecString = "0204519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574";

            Assert.AreEqual(expectedSecString, Serialisation.GetHexFromBytes(pk.ToCompressedSecFormat()));
        }

        #region Parse
        [TestMethod]
        public void Parse_invalidMarkerByteThrows()
        {
            var buffer = new byte[] { 255, 0, 0 };
            Assert.ThrowsException<ArgumentException>(() => PublicKey.Parse(buffer));
        }

        [TestMethod]
        public void Parse_uncompressedBufferIncorrectLengthThrows()
        {
            var buffer = new byte[] { 4 };
            Assert.ThrowsException<ArgumentException>(() => PublicKey.Parse(buffer));
        }

        [TestMethod]
        public void Parse_compressedBufferIncorrectLengthThrows()
        {
            var buffer = new byte[] { 2 };
            Assert.ThrowsException<ArgumentException>(() => PublicKey.Parse(buffer));
        }

        [TestMethod]
        public void Parse_uncompressedBufferValid()
        {
            var uncompressedSecString = "0479BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
            var uncompressedSecBytes = Serialisation.GetBytesFromHex(uncompressedSecString);

            var expectedX = Secp256k1.Gx;
            var expectedY = Secp256k1.Gy;
            var pk = PublicKey.Parse(uncompressedSecBytes);
            var actualX = pk.Point.X.Value;
            var actualY = pk.Point.Y.Value;

            Assert.AreEqual(expectedX, actualX);
            Assert.AreEqual(expectedY, actualY);
        }

        [TestMethod]
        public void Parse_compressedBufferValid()
        {
            var compressedSecString = "0204519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574";
            var compressedSecBytes = Serialisation.GetBytesFromHex(compressedSecString);

            var expectedX = BigInteger.Parse("0004519fac3d910ca7e7138f7013706f619fa8f033e6ec6e09370ea38cee6a7574", NumberStyles.AllowHexSpecifier);
            var expectedY = BigInteger.Parse("0082b51eab8c27c66e26c858a079bcdf4f1ada34cec420cafc7eac1a42216fb6c4", NumberStyles.AllowHexSpecifier);
            var pk = PublicKey.Parse(compressedSecBytes);
            var actualX = pk.Point.X.Value;
            var actualY = pk.Point.Y.Value;

            Assert.AreEqual(expectedX, actualX);
            Assert.AreEqual(expectedY, actualY);
        }
        #endregion
    }
}
