using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitcoinMaths;

namespace BitcoinMathsTest
{
    [TestClass]
    public class HashTests
    {
        //https://en.bitcoin.it/wiki/Protocol_documentation#Hashes
        [TestMethod]
        public void DoubleSha256_helloBytes()
        {
            var helloBytes = Encoding.UTF8.GetBytes("hello");

            var expectedHash = "9595c9df90075148eb06860365df33584b75bff782a510c6cd4883a419833d50".GetBytesFromHex();
            var actualHash = Hash.DoubleSha256(helloBytes);

            CollectionAssert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void DoubleSha256_helloString()
        {
            var helloString = "hello";

            var expectedHash = "9595c9df90075148eb06860365df33584b75bff782a510c6cd4883a419833d50".GetBytesFromHex();
            var actualHash = Hash.DoubleSha256(helloString);

            CollectionAssert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void Hash160_result()
        {
            var preimage = "02b4632d08485ff1df2db55b9dafd23347d1c47a457072a1e87be26896549a8737".GetBytesFromHex();

            var expectedHash = "93ce48570b55c42c2af816aeaba06cfee1224fae".GetBytesFromHex();
            var actualHash = preimage.Hash160();

            CollectionAssert.AreEqual(expectedHash, actualHash);
        }
    }
}
