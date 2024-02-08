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
    public class SettlementLogRepository : Repository<EgsSettlementLog>, ISettlementLogRepository
    {
        ApplicationDBContext _context;

        public SettlementLogRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsSettlementLog> GetLogByID(int id)
        {
            try
            {
                var settlementLogDetails = await Context.EGS_SettlementLog.Where(p => p.SettlementLogID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return settlementLogDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
