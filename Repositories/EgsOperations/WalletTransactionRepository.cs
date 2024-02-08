using PaymentGateway21052021.Models;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Areas.Reports.Models;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class WalletTransactionRepository : Repository<EgsWalletTransaction>, IWalletTransactionRepository
    {

        ApplicationDBContext _context;

        public WalletTransactionRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsWalletTransaction> GetTransaction(int id)
        {
            try
            {
                var walletTransactionDetails = await Context.EGS_WalletTransaction.Where(p => p.WalletTransactionID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return walletTransactionDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRange(DateTime start, DateTime end)
        {
            IEnumerable<EgsWalletTransaction> transData = null;
            try
            {
                transData = await Context.EGS_WalletTransaction.Where(x => x.TransactionDate >= start && x.TransactionDate <= end)
                            .AsNoTracking()
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRangeAndUserID(DateTime start, DateTime end, int ID)
        {
            IEnumerable<EgsWalletTransaction> transData = null;
            try
            {
                transData = await Context.EGS_WalletTransaction.Where(x => x.TransactionDate.Date >= start && x.TransactionDate.Date <= end && x.Wallet.User.UserID == ID)
                            .Include(usr => usr.Wallet)
                            .AsNoTracking()
                            .ToListAsync();
                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsWalletTransaction>> GetWalletInfoByUser(int ID)
        {
            IEnumerable<EgsWalletTransaction> transData = null;
            try
            {
                transData = await Context.EGS_WalletTransaction.Where(x => x.Wallet.User.UserID == ID)
                            .Include(usr => usr.Wallet)
                            .AsNoTracking()
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsWalletTransaction>> GetTransactionswithDateRangeUserID(DateTime start, DateTime end, int ID)
        {
            List<EgsWalletTransaction> transData = null;
            try
            {
                transData = await Context.EGS_WalletTransaction.Where(x => x.TransactionDate.Date >= start && x.TransactionDate.Date <= end
                && x.CreatedBy == ID)
                            .Include(usr => usr.Wallet)
                             .Include(h => h.TransactionType)
                            .Include(c => c.TransactionMethod)
                            .AsNoTracking()
                            .ToListAsync();
                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<TransactionsModel>> GetBackOfficeTransactions(int productID,DateTime startPeriod, DateTime endPeriod)
        {
            IEnumerable<TransactionsModel> transData = null;
            //var query = dbContext.Table1.FromSql("SELECT ISNULL(Column1, 'AliasName') AS ColumnAlias FROM Table1");
            var dtparam = ""; int dtparam2;
            //var results = query.ToList();ProductItemID	ProductID
            //1686    49
            if (productID > 0)
            {
                dtparam = " AND ProductItemID IN (select ProductItemID from EGS_ProductItem WHERE ProductID = {2}) ";
                //dtparam2 = productID;
            }
            transData = await Context.EGS_WalletTransaction
                            //await Context.EGS_WalletTransaction.Where(x => x.Wallet.User.UserID == ID)
                            .Include(prd => prd.Invoices)
                            .ThenInclude(prd => prd.Product)
                            .Include(prd => prd.ProductItem)
                            .Include(prd => prd.TransactionType)
                            .Include(prd => prd.TransactionMethod)
                            .Include(prd => prd.Wallet)
                            .FromSql($"select * from EGS_WalletTransaction WHERE (Convert(Date,TransactionDate) BETWEEN {{0}} AND {{1}}) {dtparam}", startPeriod.Date, endPeriod.Date, productID)
                            .Select(prd => new TransactionsModel
                            {
                                WalletReferenceNumber = prd.WalletReferenceNumber,
                                TransactionReferenceNo = prd.Invoices.AlternateReferenceNo == null ? prd.TransactionReferenceNo : prd.Invoices.AlternateReferenceNo,
                                TransactionType = prd.TransactionType.TransactionType,
                                TransactionMethod = prd.TransactionMethod.TransactionMethod,
                                ProductItem = prd.ProductItem.ProductItemName == null ? "Funding" : prd.ProductItem.ProductItemName,
                                CreatedBy = prd.CreatedBy,
                                Amount = prd.Amount,
                                Previous = prd.Previous,
                                Current = prd.Current,
                                SurCharge = prd.SurCharge,
                                TransactionDescription = "",//prd.TransactionDescription == null ? "--" : prd.TransactionDescription,
                                PaymentDate = prd.TransactionDate,// == null ? prd.TransactionReferenceNo : prd.Invoices.AlternateReferenceNo,
                                PaymentStatus = prd.Invoices.PaymentStatus == false ? true : true,
                                CreatedByName = prd.Wallet.User.FirstName + " " + prd.Wallet.User.LastName,
                                Region = prd.Wallet.User.ResidentialState.StateName,
                                Email = prd.Wallet.User.Email,
                                AccountNumber = prd.Wallet.WalletAccountNumber,
                            })//.Include(inv => inv.in)
                            .OrderByDescending(x => x.TransactionDate)
                            .ToListAsync();


            return transData;

        }



    }
}
