

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
    public class SettlementHistoryRepository : Repository<EgsSettlementHistory>, ISettlementHistoryRepository
    {
        ApplicationDBContext _context;

        public SettlementHistoryRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsSettlementHistory> GetSettlementHistoryByID(int id)
        {
            try
            {
                var settlementHistoryDetails = await Context.EGS_SettlementHistory.Where(p => p.SettlementHistoryID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return settlementHistoryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task <List<EgsSettlementHistory>> GetSettlementHistory()
        {
            try
            {
                var settlementHistoryDetails = await Context.EGS_SettlementHistory
                            //.AsNoTracking()
                            .ToListAsync();

                return settlementHistoryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
