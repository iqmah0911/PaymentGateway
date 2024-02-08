
using PaymentGateway21052021.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface ISettlementHistoryRepository : IRepository<EgsSettlementHistory>
    {
        Task<EgsSettlementHistory> GetSettlementHistoryByID(int id);
        Task<List<EgsSettlementHistory>> GetSettlementHistory();

       // Task GetSettlementHistoryByID();
        //  Task GetAllSettlementHistory();

        //  Task<List<EgsSettlementHistory>> GetSettlementHistory();

    }
}
