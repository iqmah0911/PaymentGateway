using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class UssdTransactionsRepository : Repository<EgsUssdTransactions>, IUssdTransactionsRepository
    {
        private readonly ApplicationDBContext _context;

        public UssdTransactionsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context; 
        }


        public async Task<EgsUssdTransactions> GetUssdTransactionsByID(int id)
        {
            var ussdTransaction = await Context.EGS_UssdTransactions.Where(x => x.ussdTransactionID == id)
                                                              .AsNoTracking()
                                                              .FirstOrDefaultAsync();

            return ussdTransaction;
        }
    }
}
