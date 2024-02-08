using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IUpgradeAccountRepository : IRepository<EgsUpgradeAccount>
    {
        Task<EgsUpgradeAccount> GetRequestByUserId(int id);
        Task<IEnumerable<EgsUpgradeAccount>> GetAllRequests();
        Task<EgsUpgradeAccount> GetRequestById(int id);
       // Task<IEnumerable<EgsUpgradeAccount>> GetAllAgentRequest();
    }
}
