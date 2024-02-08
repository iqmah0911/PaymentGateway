//using PaymentGateway.Repositories;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface ITransactionLimitRepository : IRepository<SysTransactionLimit>
    {
        Task<List<SysTransactionLimit>> GetAllTransactionLimit();
        List<SysTransactionLimit> GetAllForDropdown();
        Task<SysTransactionLimit> GetTransactionLimit(int id);
        List<SysTransactionLimit> GetAllForDropdownList();

    }
}
