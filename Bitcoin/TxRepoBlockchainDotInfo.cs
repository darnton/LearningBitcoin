using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BitcoinMaths;

namespace Bitcoin
{
    public class TxRepoBlockchainDotInfo : ITxRepo
    {
        private static Dictionary<string, Transaction> TxCache = new Dictionary<string, Transaction>();

        public async Task<Transaction> FetchAsync(string txId, Network network = Network.MainNet, bool force = false)
        {
            if (force || !TxCache.ContainsKey(txId))
            {
                var url = $"https://blockchain.info/rawtx/{txId}?format=hex";
                var client = new HttpClient();
                var txBytes = (await client.GetStringAsync(url)).GetBytesFromHex();
                var tx = Transaction.Parse(new BinaryReader(new MemoryStream(txBytes)), this);

                var expectedTxId = txId.GetBytesFromHex().Reverse().ToArray().EncodeAsHex();
                if (tx.Id != expectedTxId)
                {
                    throw new ValidationException($"Transaction id doesn't match. Expecting {txId}; was {tx.Id}.");
                }

                TxCache[txId] = tx;
            }
            return TxCache[txId];
        }
    }
}
