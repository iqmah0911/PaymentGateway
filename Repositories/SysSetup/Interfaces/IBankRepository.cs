using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IBankRepository : IRepository<SysBank>
    {
        Task<SysBank> GetBanks(int id);
        Task<List<SysBank>> GetAllBanks();
        Task<SysBank> GetBanksByCode(string code);
    }
}
