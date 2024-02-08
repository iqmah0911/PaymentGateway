using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface ICabletvusersRepository : IRepository<EgsCabletvusers>
    {
        Task<EgsCabletvusers> GetCableusersByID(int id);
    }
}
