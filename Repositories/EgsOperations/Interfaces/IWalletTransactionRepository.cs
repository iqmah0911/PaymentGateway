using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IWalletTransactionRepository : IRepository<EgsWalletTransaction>
    {
        Task<EgsWalletTransaction> GetTransaction(int id);
        Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRange(DateTime start, DateTime end);
        Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRangeAndUserID(DateTime start, DateTime end, int ID);
        Task<IEnumerable<EgsWalletTransaction>> GetWalletInfoByUser(int ID);
        Task<List<EgsWalletTransaction>> GetTransactionswithDateRangeUserID(DateTime start, DateTime end, int ID);
        Task<IEnumerable<TransactionsModel>> GetBackOfficeTransactions(int productID, DateTime startPeriod, DateTime endPeriod);
    }
}
