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
    public class SettlementModeRepository : Repository<EgsSettlementMode>, ISettlementModeRepository
    {
        ApplicationDBContext _context;

        public SettlementModeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

 

        public async Task<EgsSettlementMode> GetSettlementModeByID(int id)
        {
            try
            {
                var settlementMode = await Context.EGS_SettlementMode.Where(p => p.SettlementModeID == id)
                                     .FirstOrDefaultAsync();

                return settlementMode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }








    }
}
