using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface ISettlementBasisRepository : IRepository<EgsSettlementBasis>
    {
        Task<EgsSettlementBasis> GetSettlementByID(int id);

        Task<EgsSettlementBasis> GetSettlementBasisByProductID(int id);
        Task<EgsSettlementBasis> GetSettlementBasisByProductItemID(int productid, int merchantid, int itemid);

    }
}
