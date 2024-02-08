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
    public class BeneficiaryRepository : Repository<EgsBeneficiary>, IBeneficiaryRepository
    {
        ApplicationDBContext _context;

        public BeneficiaryRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EgsBeneficiary>> GetBeneficiaryByUserID(int id)
        {
            try
            {
                var BeneficiaryDetails = await Context.EGS_Beneficiary.Where(p => p.userId == id) 
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .ToListAsync();

                return BeneficiaryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsBeneficiary> GetBeneficiaryByAcct(string accnum)
        {
            try
            {
                var BeneficiaryDetail = await Context.EGS_Beneficiary.Where(p => p.BeneficiaryBankAccount == accnum)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return BeneficiaryDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsBeneficiary> GetBeneficiaryByBenfID(int id)
        {
            try
            {
                var BeneficiaryDetails = await Context.EGS_Beneficiary.Where(p => p.BeneficaryID == id)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return BeneficiaryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
