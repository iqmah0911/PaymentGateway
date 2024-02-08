using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IPinpadsRepository : IRepository<EgsPinpads>
    {
        Task<EgsPinpads> GetPinPadsByID(int id);
    }
}
