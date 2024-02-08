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
    public class TransactionMethodRepository : Repository<EgsTransactionMethod>, ITransactionMethodRepository
    {
        ApplicationDBContext _context;

        public TransactionMethodRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EgsTransactionMethod>> GetAllTransactionMethod()
        {
            try
            {
                var allTransactionMethod = await Context.EGS_TransactionMethod
                            .AsNoTracking()
                            .ToListAsync();

                return allTransactionMethod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsTransactionMethod> GetTransactionMehodByID(int Id)
        {
            try
            {
                var tranMetd = await Context.EGS_TransactionMethod
                                 .Where(u => u.TransactionMethodID == Id) 
                                 .SingleOrDefaultAsync();

                return tranMetd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsTransactionMethod> GetTransactionMethodByName(string Name)
        {
            try
            {
                var tranMetd = await Context.EGS_TransactionMethod
                                 .Where(u => u.TransactionMethod == Name)
                                 .FirstOrDefaultAsync();

                return tranMetd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
