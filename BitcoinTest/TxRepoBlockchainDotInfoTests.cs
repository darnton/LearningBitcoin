using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bitcoin;

namespace BitcoinTest
{
    [TestClass]
    public class TxRepoBlockchainDotInfoTests
    {
        [TestMethod]
        public void FetchAsync_pizzaTxFetched()
        {
            //Famous transaction ids
            var pizzaTxId = "a1075db55d416d3ca199f55b6084e2115b9345e16c5cf302fc80e9d5fbf5d48d";

            var repo = new TxRepoBlockchainDotInfo();
            var tx = repo.FetchAsync(pizzaTxId).GetAwaiter().GetResult();

            Assert.AreEqual(131, tx.Inputs.Length);
            Assert.AreEqual(1, tx.Outputs.Length);
            Assert.AreEqual((ulong)1000000000000, tx.Outputs[0].Amount);
        }
    }
}
