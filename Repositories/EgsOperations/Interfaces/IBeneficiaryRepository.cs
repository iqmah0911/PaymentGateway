using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IBeneficiaryRepository : IRepository<EgsBeneficiary>
    {
        Task<List<EgsBeneficiary>> GetBeneficiaryByUserID(int id);

        Task<EgsBeneficiary> GetBeneficiaryByBenfID(int id);

        Task<EgsBeneficiary> GetBeneficiaryByAcct(string accnum);

    }
}
