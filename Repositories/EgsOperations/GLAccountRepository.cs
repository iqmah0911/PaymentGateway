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
    public class GLAccountRepository : Repository<EgsGLAccount>, IGLAccountRepository
    {
        ApplicationDBContext _context;

        public GLAccountRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsGLAccount> GetGLAccount(int id)
        {
            try
            {
                var walletDetails = await Context.EGS_GLAccounts.Where(p => p.GLAccountID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return walletDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<EgsGLAccount>> GetAllGLAccounts()
        {
            try
            {
                var allBank = await Context.EGS_GLAccounts
                            .Include(x => x.Company)
                            .Include(pc => pc.Bank)
                            .AsNoTracking()
                            .ToListAsync();

                return allBank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsGLAccount> GetGLAccountByCompanyID(int id) // Sending in the GLAccountID 
        {
            try
            {
                var accountDetails = await Context.EGS_GLAccounts.Where(p => p.GLAccountID == id)
                            .Include(x => x.Company)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return accountDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsGLAccount> GetGLAccountByBankID(int id)
        {
            try
            {
                var accountDetails = await Context.EGS_GLAccounts.Where(p => p.GLAccountID == id)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return accountDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsGLAccount> GetGLAccountInUse()
        {
            try
            {
                var accountDetails = await Context.EGS_GLAccounts.Where(p => p.IsActive == true && p.IsInUse == true)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return accountDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
