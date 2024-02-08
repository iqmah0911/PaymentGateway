using PaymentGateway21052021.Models;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IBettingUsersRepository : IRepository<EgsBettingUsers>
    {
        Task<EgsBettingUsers> GetBettingUsersByID(int id);
    }
}
