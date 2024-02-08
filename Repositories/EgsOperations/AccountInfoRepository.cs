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
    public class AccountInfoRepository : Repository<EgsAccountsInfo>, IAccountInfoRepository
    {

        ApplicationDBContext _context;

        public AccountInfoRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsAccountsInfo> GetAccountInfo(int id)
        {
            try
            {
                var itemDetails = await Context.EGS_AccountInfo.Where(p => p.AccountsInfoID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsAccountsInfo>> GetAllAccountInfos()
        {
            try
            {
                var allAccountInfos = await Context.EGS_AccountInfo
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .ToListAsync();
                return allAccountInfos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsAccountsInfo> GetAccountInfosWithBankId(int id) // Sending in the AccountsInfoID id
        {
            try
            {
                var itemDetails = await Context.EGS_AccountInfo.Where(p => p.AccountsInfoID == id)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
