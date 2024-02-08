using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class BettingUsersRepository : Repository<EgsBettingUsers>, IBettingUsersRepository
    {
        private readonly ApplicationDBContext _context;

        public BettingUsersRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsBettingUsers> GetBettingUsersByID(int id)
        {
            var bettingusers = await Context.EGS_BettingUsers.Where(x => x.bettingID == id)
                                                       .AsNoTracking()
                                                       .FirstOrDefaultAsync();

            return bettingusers;
        }
    }
}
