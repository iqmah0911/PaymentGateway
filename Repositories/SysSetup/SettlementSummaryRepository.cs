using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class SettlementSummaryRepository : Repository<EgsSettlementSummary>, ISettlementSummaryRepository
    {
        ApplicationDBContext _context;

        public SettlementSummaryRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EgsSettlementSummary>> GetSettlementUnPaid()
        {
            try
            {
                var settlementDetails = await Context.EGS_SettlementSummary.Where(x => x.IsPaid == false)
                            .Include(m => m.Merchant)
                            .AsNoTracking()
                            .ToListAsync();

                return settlementDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
