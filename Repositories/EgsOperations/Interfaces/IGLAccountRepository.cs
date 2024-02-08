using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IGLAccountRepository : IRepository<EgsGLAccount>
    {
        Task<EgsGLAccount> GetGLAccount(int id);
        Task<List<EgsGLAccount>> GetAllGLAccounts();
        Task<EgsGLAccount> GetGLAccountByCompanyID(int id);
        Task<EgsGLAccount> GetGLAccountByBankID(int id); 
        Task<EgsGLAccount> GetGLAccountInUse();
    }
}
