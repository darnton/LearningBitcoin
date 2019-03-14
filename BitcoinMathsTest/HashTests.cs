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
            var expectedHex = "9595c9df90075148eb06860365df33584b75bff782a510c6cd4883a419833d50";

            var hash = Hash.DoubleSha256(helloBytes);
            var actualHex = string.Join("", hash.Select(b => $"{b:x2}"));

            Assert.AreEqual(expectedHex, actualHex);
        }

        [TestMethod]
        public void DoubleSha256_helloString()
        {
            var helloString = "hello";
            var expectedHex = "9595c9df90075148eb06860365df33584b75bff782a510c6cd4883a419833d50";

            var hash = Hash.DoubleSha256(helloString);
            var actualHex = string.Join("", hash.Select(b => $"{b:x2}"));

            Assert.AreEqual(expectedHex, actualHex);
        }

        [TestMethod]
        public void Hash160_result()
        {
        }
    }
}
