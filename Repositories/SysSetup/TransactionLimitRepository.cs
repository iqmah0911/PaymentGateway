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
    public class TransactionLimitRepository : Repository<SysTransactionLimit>, ITransactionLimitRepository
    {
        ApplicationDBContext _context;

        public TransactionLimitRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        //TransactionLimit



        public async Task<SysTransactionLimit> GetTransactionLimit(int id)
        {
            try
            {
                var transactionLimitDetails = await Context.SYS_TransactionLimits.Where(p => p.TransactionLimitID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return transactionLimitDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SysTransactionLimit>> GetAllTransactionLimit()
        {
            try
            {
                var allTransactionLimit = await Context.SYS_TransactionLimits
                            .AsNoTracking()
                            .ToListAsync();

                return allTransactionLimit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<SysTransactionLimit> GetAllForDropdown()
        {
            return Context.SYS_TransactionLimits.ToList();
        }


        public List<SysTransactionLimit> GetAllForDropdownList()
        {
            return Context.SYS_TransactionLimits.ToList();
        }

    }
}
