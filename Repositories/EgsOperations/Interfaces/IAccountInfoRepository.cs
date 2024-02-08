using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IAccountInfoRepository : IRepository<EgsAccountsInfo>
    {
        Task<EgsAccountsInfo> GetAccountInfosWithBankId(int id);
        Task<List<EgsAccountsInfo>> GetAllAccountInfos();
        Task<EgsAccountsInfo> GetAccountInfo(int id);
    }
}
