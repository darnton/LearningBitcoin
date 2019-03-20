using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BitcoinMaths;

namespace Bitcoin
{
    public class TxRepoProgrammingBitcoinDotCom : ITxRepo
    {
        private static Dictionary<string, Transaction> TxCache = new Dictionary<string, Transaction>();

        public async Task<Transaction> FetchAsync(string txId, Network network = Network.MainNet, bool force = false)
        {
            if (force || !TxCache.ContainsKey(txId))
            {
                var subdomain = (network == Network.TestNet) ? "testnet" : "mainnet";
                var url = $"http://{subdomain}.programmingbitcoin.com/tx/{txId}.hex";
                var client = new HttpClient();
                var txBytes = (await client.GetStringAsync(url)).GetBytesFromHex();
                var tx = Transaction.Parse(new BinaryReader(new MemoryStream(txBytes)));

                if (tx.Id != txId)
                {
                    throw new ValidationException($"Transaction id doesn't match. Expecting {txId}; was {tx.Id}.");
                }

                TxCache[txId] = tx;
            }
            return TxCache[txId];
        }
    }
}
