using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.Reports.Interfaces
{
    public interface IWalletHistoryRepository: IRepository<EgsWalletTransaction>
    {
        Task<IEnumerable<EgsWalletTransaction>> GetWalletHistoryTransaction(int userId,int walletId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus);
        Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRange(DateTime start, DateTime end);
        Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRangeAndUserID(DateTime start, DateTime end, int ID);
        Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByIDAndValue( int ID, int Value);
        Task<IEnumerable<EgsWalletTransaction>> GetWalletTransaction(int userId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus);


    }
}
