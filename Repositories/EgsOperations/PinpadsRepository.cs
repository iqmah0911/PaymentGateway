using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class PinpadsRepository : Repository<EgsPinpads>, IPinpadsRepository
    {
        private readonly ApplicationDBContext _context;

        public PinpadsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsPinpads> GetPinPadsByID(int id)
        {
            var pinpads = await Context.EGS_Pinpads.Where(x => x.pinpadsID == id)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync();

            return pinpads;
        }

    }
}
