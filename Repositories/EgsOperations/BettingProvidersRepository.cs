using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class BettingProvidersRepository : Repository<EgsBettingProviders>, IBettingProvidersRepository
    {
        private readonly ApplicationDBContext _context;

        public BettingProvidersRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsBettingProviders> GetBettingProvidersByID(int id)
        {
            var bettingproviders = await Context.EGS_BettingProviders.Where(x => x.bettingID == id)
                                                                .AsNoTracking()
                                                                .FirstOrDefaultAsync();
                                                                
            return bettingproviders;
        }
       
    }
}
