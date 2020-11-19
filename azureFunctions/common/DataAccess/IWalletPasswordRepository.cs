using SupplyChain.Common.Models;
using System.Threading.Tasks;

namespace SupplyChain.Common.DataAccess
{
    public interface IWalletPasswordRepository : IRepository<WalletPassword>
    {
        Task<WalletPassword> GetByUserId(string id);
    }
}
