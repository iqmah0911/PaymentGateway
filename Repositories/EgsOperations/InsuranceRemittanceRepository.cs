using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class InsuranceRemittanceRepository : Repository<EgsInsuranceRemittance>, IInsuranceRemittanceRepository
    {
         
            ApplicationDBContext _context;

            public InsuranceRemittanceRepository(ApplicationDBContext context) : base(context)
            {
                _context = context;
            }


       public async Task<EgsInsuranceRemittance> GeRemittance(int id)
        {
            try
            {
                var remDetails = await Context.EGS_InsuranceRemittance.Where(p => p.InsuranceID == id) 
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return remDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsInsuranceRemittance>> GeRemittancelist(DateTime transdate)
        {
            try
            {
                List<EgsInsuranceRemittance> remlist = new List<EgsInsuranceRemittance>();
                remlist = await Context.EGS_InsuranceRemittance.Where(p => p.TransactionDate == transdate)
                            .AsNoTracking()
                            .ToListAsync();

                return remlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
