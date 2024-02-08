using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface ITransactionMethodRepository
    {
        Task<List<EgsTransactionMethod>> GetAllTransactionMethod();
        Task<EgsTransactionMethod> GetTransactionMehodByID(int Id);
        Task<EgsTransactionMethod> GetTransactionMethodByName(string Name);
    }
}
