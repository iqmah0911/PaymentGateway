using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class WalletRepository : Repository<EgsWallet>, IWalletRepository
    {
        ApplicationDBContext _context;

        public WalletRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateInvoiceBalance(EgsWallet entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }


        public async Task<EgsWallet> GetWallet(int id)
        {
            try
            {
                var walletDetails = await Context.EGS_Wallet.Where(p => p.WalletID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return walletDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsWallet> GetWalletByUserID(int id)
        {
            try
            {
                var walletDetails = await Context.EGS_Wallet.Where(p => p.User.UserID == id)
                            .Include(usr => usr.User)
                            .Include(wusr=> wusr.User.Wallet)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return walletDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsWallet> GetWalletByAccountNumber(string WalletAccountNumber)
        {
            try
            {
                var walletDetails = await Context.EGS_Wallet.Where(p => p.WalletAccountNumber == WalletAccountNumber)
                            .Include(usr => usr.User)
                            .Include(wuser=>wuser.User.Wallet)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return walletDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsWallet>> GetWallets()
        {
            try
            {
                var wallets = await Context.EGS_Wallet
                            .Include(x => x.User)
                            .AsNoTracking()
                            .ToListAsync();

                return wallets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsWallet> GetUserByWalletID(int id)
        {
            try
            {
                var walletDetails = await Context.EGS_Wallet.Where(p => p.WalletID == id)
                            .Include(usr => usr.User)
                            .Include(usracttype => usracttype.User.AccountType)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return walletDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsWallet> GetWalletBalanceById(int id)
        {
            try
            {
                var wallets = await Context.EGS_Wallet
                            .Include(x => x.User)
                            .Where(p => p.WalletID == id)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();
                //.SingleOrDefaultAsync();

                return wallets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsWallet> GetAWalletByUserID(int id)
        {
            try
            {
                var walletDetails = await Context.EGS_Wallet.Where(p => p.User.UserID == id)
                            .Include(usr => usr.User)
                            .Include(rusr => rusr.User.Role)
                            .Include(wusr => wusr.User.Wallet)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return walletDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
