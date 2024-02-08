//using PaymentGateway.Repositories;
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
    public class SettlementTypeRepository : Repository<EgsSettlementType>, ISettlementTypeRepository
    {
        ApplicationDBContext _context;

        public SettlementTypeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task<List<EgsSettlementType>> GetSettlementTypes()
        {
            try
            {
                var allSettlementTypes = await Context.EGS_SettlementType
                            .AsNoTracking()
                            .ToListAsync();
                return allSettlementTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         


        public async Task<EgsSettlementType> GetSettlementTypeByID(int id)
        {
            try
            {
                var settlementTypeDetails = await Context.EGS_SettlementType.Where(p => p.SettlementTypeID == id)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return settlementTypeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
