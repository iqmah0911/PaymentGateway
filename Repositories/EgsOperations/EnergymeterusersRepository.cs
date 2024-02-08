using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class EnergymeterusersRepository : Repository<EgsEnergymeterusers>, IEnergymeterusersRepository
    {
        private readonly ApplicationDBContext _context;

        public EnergymeterusersRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsEnergymeterusers> GetEnergymeterByID(int id)
        {
            var energymeter = await Context.EGS_Energymeterusers.Where(x => x.energymeterID == id)
                                                          .AsNoTracking()
                                                          .FirstOrDefaultAsync();

            return energymeter;
        }
    }
}
