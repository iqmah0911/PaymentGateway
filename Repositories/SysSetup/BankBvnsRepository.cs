using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class BankBvnsRepository : Repository<EgsBankBvns>, IBankBvnsRepository
    {
        private readonly ApplicationDBContext _context;

        public BankBvnsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsBankBvns> GetBankBvnsByID(int id)
        {
            var bankbvns = await Context.EGS_BankBvns.Where(x => x.bankbvnsID == id)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync();
            return bankbvns;

        }
    }
}
