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
    public class SettlementIntervalRepository : Repository<EgsSettlementInterval>,ISettlementIntervalRepository 
    {
        ApplicationDBContext _context;

        public SettlementIntervalRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<EgsSettlementInterval>> GetSettlementIntervals()
        {
            try
            {
                var allSettlementIntervals = await Context.EGS_SettlementInterval
                            .AsNoTracking()
                            .ToListAsync();
                return allSettlementIntervals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsSettlementInterval> GetSettlementIntervalById(int id)
        {
            try
            {
                var settlementInterDetails = await Context.EGS_SettlementInterval.Where(p => p.SettlementIntervalID == id)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return settlementInterDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
