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
    public class AccountTypeRepository : Repository<EgsAccountType>, IAccountTypeRepository
    {
        ApplicationDBContext _context;

        public AccountTypeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EgsAccountType>> GetAllAccountTypes()
        {
            try
            {
                var acctType = await Context.EGS_AccountType
                            .AsNoTracking()
                            .ToListAsync();
                return acctType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsAccountType> GetAccountTypeByID(int id)
        {
            try
            {
                var acctType = await Context.EGS_AccountType.Where(acc => acc.AccountTypeID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();
                return acctType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
