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
    public class BankRepository : Repository<SysBank>, IBankRepository
    {
        ApplicationDBContext _context;

        public BankRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<SysBank> GetBanks(int id)
        {
            try
            {
                var bankDetails = await Context.SYS_Bank.Where(p => p.BankID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return bankDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysBank>> GetAllBanks()
        {
            try
            {
                var allBank = await Context.SYS_Bank.OrderBy(x => x.BankName)
                            .AsNoTracking()
                            .ToListAsync();

                return allBank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SysBank> GetBanksByCode(string code)
        {
            try
            {
                var bankDetails = await Context.SYS_Bank.Where(p => p.BankCode == code)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return bankDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
