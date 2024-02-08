using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IWalletRepository : IRepository<EgsWallet>
    {
        Task<EgsWallet> GetWallet(int id);
        Task<List<EgsWallet>> GetWallets();
        Task<EgsWallet> GetUserByWalletID(int id);

        Task<EgsWallet> GetWalletByUserID(int id);
        Task<EgsWallet> GetWalletByAccountNumber(string WalletAccountNumber);
        Task<EgsWallet> GetWalletBalanceById(int id);
        Task<EgsWallet> GetAWalletByUserID(int id);

    }
}
