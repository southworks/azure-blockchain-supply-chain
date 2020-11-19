using SupplyChain.Common.Models;
using System.Threading.Tasks;

namespace SupplyChain.Common.DataAccess
{
    public interface IRepository<T> where T : class
    {
        Task Initialize();

        Task<T> GetById(string id);

        Task Insert(T userProfile);

        Task Delete(string id);

        Task Update(string id, T userProfile);
    }
}
