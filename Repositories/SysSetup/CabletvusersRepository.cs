using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class CabletvusersRepository : Repository<EgsCabletvusers>, ICabletvusersRepository
    {
        private readonly ApplicationDBContext _context;
        

        public CabletvusersRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsCabletvusers> GetCableusersByID(int id)
        {
            var cableusers = await Context.EGS_cabletvusers.Where(x => x.CableID == id)
                                                     .AsNoTracking()
                                                     .FirstOrDefaultAsync();

            return cableusers;
        }
    }
}
