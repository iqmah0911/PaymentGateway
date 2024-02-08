using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IBankBvnsRepository : IRepository<EgsBankBvns>
    {
        Task<EgsBankBvns> GetBankBvnsByID(int id);
    }
}
