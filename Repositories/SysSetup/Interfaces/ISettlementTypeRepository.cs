using PaymentGateway21052021.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface ISettlementTypeRepository : IRepository<EgsSettlementType>
    {

        Task<List<EgsSettlementType>> GetSettlementTypes();
        Task<EgsSettlementType> GetSettlementTypeByID(int id);

    }
}
