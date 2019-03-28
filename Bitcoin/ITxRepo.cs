using System.Threading.Tasks;

namespace Bitcoin
{
    public interface ITxRepo
    {
        Task<Transaction> FetchAsync(string txId, Network network = Network.MainNet, bool force = false);
    }
}
