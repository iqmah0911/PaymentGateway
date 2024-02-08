using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class AccountManagersRepository : Repository<EgsAccountManagers>, IAccountManagersRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountManagersRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<EgsAccountManagers> GetAccountManagersByID(int id)
        {
            try
            {
                var accountmanagers = await Context.EGS_AccountManagers.Where(x => x.accountManagerID == id)
                                                                 .AsNoTracking()
                                                                 .FirstOrDefaultAsync();

                return accountmanagers;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
