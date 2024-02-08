using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IAccountManagersRepository: IRepository<EgsAccountManagers>
    {
        Task<EgsAccountManagers> GetAccountManagersByID(int id);
    }
}
