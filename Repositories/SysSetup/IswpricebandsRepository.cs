using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class IswpricebandsRepository : Repository<EgsIswpricebands>, IIswpricebandsRepository
    {
        private readonly ApplicationDBContext _context;

        public IswpricebandsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsIswpricebands> GetIswPriceByID(int id)
        {
            var iswprice = await Context.EGS_Iswpricebands.Where(x => x.IswpriceID == id)
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync();

            return iswprice;
        }
    }
}
