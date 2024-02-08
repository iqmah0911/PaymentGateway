using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IEnergymeterusersRepository : IRepository<EgsEnergymeterusers>
    {
        Task<EgsEnergymeterusers> GetEnergymeterByID(int id);
    }
}
