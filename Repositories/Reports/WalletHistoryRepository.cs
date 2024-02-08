using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PaymentGateway21052021.Repositories.Reports
{
    public class WalletHistoryRepository : Repository<EgsWalletTransaction>, IWalletHistoryRepository
    {
        ApplicationDBContext _context;

        public WalletHistoryRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<EgsWalletTransaction>> GetWalletHistoryTransaction(int userId, int walletId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus)
        {
            IEnumerable<EgsWalletTransaction> transData = null;
            SysUsers userDetails = null;

            if (queryStatus.ToUpper() == "ALL")
            {
                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN") || roleName.ToUpper().Contains("OPERATIONS") || roleName.ToUpper().Contains("CUSTOMER RECONCILATION"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                           .Include(u => u.Role)
                           .Include(u => u.Wallet)
                           .Where(p => p.UserID == userId)
                           .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.CreatedBy == userId)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null)
                        {
                            if (walletId != 0)
                            {
                                transData = await Context.EGS_WalletTransaction
                                   .Include(tTyp => tTyp.TransactionType)
                                   .Include(mTyp => mTyp.TransactionMethod)
                                   .Include(pItem => pItem.ProductItem)
                                   .Include(usr => usr.Wallet)
                                   .Include(wuser => wuser.Wallet.User)
                                   //.ThenInclude(u => u.User)
                                   .AsNoTracking()
                                   .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                    && x.Wallet.WalletID == walletId && x.TransactionMethod.TransactionMethodID != 10)
                                   .OrderByDescending(desc => desc.TransactionDate)
                                   .ToListAsync();
                            }
                            else if (walletId == 0)
                            {
                                transData = await Context.EGS_WalletTransaction
                                   .Include(tTyp => tTyp.TransactionType)
                                   .Include(mTyp => mTyp.TransactionMethod)
                                   .Include(pItem => pItem.ProductItem)
                                   .Include(usr => usr.Wallet)
                                   .Include(wuser => wuser.Wallet.User)
                                   //.ThenInclude(u => u.User)
                                   .AsNoTracking()
                                   .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                     && x.TransactionMethod.TransactionMethodID != 10)
                                   .OrderByDescending(desc => desc.TransactionDate)
                                   .ToListAsync();
                            }
                        }

                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                else if (roleName.ToUpper().Contains("FINANCE") || roleName.ToUpper().Contains("AUDIT AND CONTROL"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                            .Include(u => u.Role)
                            .Include(u => u.Wallet)
                            .Where(p => p.UserID == userId)
                            .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.CreatedBy == userId && x.ProductItem.ProductItemName != "Processing Fee")
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null && userDetails.Wallet.WalletTransactions != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                    .Include(tTyp => tTyp.TransactionType)
                                    .Include(mTyp => mTyp.TransactionMethod)
                                    .Include(pItem => pItem.ProductItem)
                                    .Include(usr => usr.Wallet)
                                    .Include(wuser => wuser.Wallet.User)
                                    //.ThenInclude(u => u.User)
                                    .AsNoTracking()
                                    .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.Wallet.WalletID == userDetails.Wallet.WalletID && x.ProductItem.ProductItemName != "Processing Fee")
                                    .OrderByDescending(desc => desc.TransactionDate)
                                    .ToListAsync();
                        }

                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.ProductItem.ProductItemName != "Processing Fee")
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                if (roleName.ToUpper().Contains("AGGREGATOR") || roleName.ToUpper().Contains("AGENT"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                           .Include(u => u.Role)
                           .Include(u => u.Wallet)
                           .Where(p => p.UserID == userId)
                           .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.CreatedBy == userId)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null)
                        {
                            if (walletId != 0 && queryStatus.ToUpper() == "ALL")
                            {
                                transData = await Context.EGS_WalletTransaction
                                   .Include(tTyp => tTyp.TransactionType)
                                   .Include(mTyp => mTyp.TransactionMethod)
                                   .Include(pItem => pItem.ProductItem)
                                   .Include(usr => usr.Wallet)
                                   .Include(wuser => wuser.Wallet.User)
                                   //.ThenInclude(u => u.User)
                                   .AsNoTracking()
                                   .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                    && x.Wallet.WalletID == walletId && x.TransactionMethod.TransactionMethodID != 10)
                                   .OrderByDescending(desc => desc.TransactionDate)
                                   .ToListAsync();
                            }
                           
                        } 
                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                //else //if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                //{
                //    userDetails = await Context.SYS_Users
                //           .Include(u => u.Role)
                //           .Include(u => u.Wallet)
                //           .Where(p => p.UserID == userId)
                //           .FirstOrDefaultAsync();

                //    if (userDetails.BankAccountCode != null)
                //    {
                //        transData = await Context.EGS_WalletTransaction
                //            .Include(tTyp => tTyp.TransactionType)
                //            .Include(mTyp => mTyp.TransactionMethod)
                //            .Include(pItem => pItem.ProductItem)
                //            .Include(usr => usr.Wallet)
                //            .Include(wuser => wuser.Wallet.User)
                //            //.ThenInclude(u => u.User)
                //            .AsNoTracking()
                //            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                //            && x.CreatedBy == userId)
                //            .OrderByDescending(desc => desc.TransactionDate)
                //            .ToListAsync();
                //    }
                //    else
                //    {
                //        transData = await Context.EGS_WalletTransaction
                //                .Include(tTyp => tTyp.TransactionType)
                //                .Include(mTyp => mTyp.TransactionMethod)
                //                .Include(pItem => pItem.ProductItem)
                //                .Include(usr => usr.Wallet)
                //                .Include(wuser => wuser.Wallet.User)
                //                //.ThenInclude(u => u.User)
                //                .AsNoTracking()
                //                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.Wallet.WalletID == userDetails.Wallet.WalletID)
                //                .OrderByDescending(desc => desc.TransactionDate)
                //                .ToListAsync();
                //    }
                //}
            }
            else if (queryStatus.ToUpper() == "DEBIT")
            {
                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN") || roleName.ToUpper().Contains("OPERATIONS") || roleName.ToUpper().Contains("CUSTOMER RECONCILATION"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                            .Include(u => u.Role)
                            .Include(u => u.Wallet)
                            .Where(p => p.UserID == userId)
                            .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.TransactionType.TransactionTypeID == 2 /*&& x.CreatedBy == userId*/)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null)
                        {
                            if (walletId != 0)
                            {
                                transData = await Context.EGS_WalletTransaction
                                                            .Include(tTyp => tTyp.TransactionType)
                                                            .Include(mTyp => mTyp.TransactionMethod)
                                                            .Include(pItem => pItem.ProductItem)
                                                            .Include(usr => usr.Wallet)
                                                            .Include(wuser => wuser.Wallet.User)
                                                            //.ThenInclude(u => u.User)
                                                            .AsNoTracking()
                                                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                                            && x.TransactionType.TransactionTypeID == 2 && x.Wallet.WalletID == walletId)
                                                            .OrderByDescending(desc => desc.TransactionDate)
                                                            .ToListAsync();
                            }
                           else if (walletId == 0)
                            {
                                transData = await Context.EGS_WalletTransaction
                                                            .Include(tTyp => tTyp.TransactionType)
                                                            .Include(mTyp => mTyp.TransactionMethod)
                                                            .Include(pItem => pItem.ProductItem)
                                                            .Include(usr => usr.Wallet)
                                                            .Include(wuser => wuser.Wallet.User)
                                                            //.ThenInclude(u => u.User)
                                                            .AsNoTracking()
                                                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                                            && x.TransactionType.TransactionTypeID == 2 )
                                                            .OrderByDescending(desc => desc.TransactionDate)
                                                            .ToListAsync();
                            }
                        }

                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.TransactionType.TransactionTypeID == 2)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                else if (roleName.ToUpper().Contains("FINANCE") || roleName.ToUpper().Contains("AUDIT AND CONTROL"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                            .Include(u => u.Role)
                            .Include(u => u.Wallet)
                            .Where(p => p.UserID == userId)
                            .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.TransactionType.TransactionTypeID == 2
                                /*&& x.CreatedBy == userId*/)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null && userDetails.Wallet.WalletID > 0)
                        {
                            transData = await Context.EGS_WalletTransaction
                                    .Include(tTyp => tTyp.TransactionType)
                                    .Include(mTyp => mTyp.TransactionMethod)
                                    .Include(pItem => pItem.ProductItem)
                                    .Include(usr => usr.Wallet)
                                    .Include(wuser => wuser.Wallet.User)
                                    //.ThenInclude(u => u.User)
                                    .AsNoTracking()
                                    .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                    && x.TransactionType.TransactionTypeID == 2 /*&& x.Wallet.WalletID == userDetails.Wallet.WalletID*/)
                                    .OrderByDescending(desc => desc.TransactionDate)
                                    .ToListAsync();
                        }

                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.TransactionType.TransactionTypeID == 2)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                if (roleName.ToUpper().Contains("AGGREGATOR") || roleName.ToUpper().Contains("AGENT"))
                { 
                    if (walletId != 0 && queryStatus.ToUpper() == "DEBIT")
                    {
                        transData = await Context.EGS_WalletTransaction
                                                          .Include(tTyp => tTyp.TransactionType)
                                                          .Include(mTyp => mTyp.TransactionMethod)
                                                          .Include(pItem => pItem.ProductItem)
                                                          .Include(usr => usr.Wallet)
                                                          .Include(wuser => wuser.Wallet.User)
                                                          //.ThenInclude(u => u.User)
                                                          .AsNoTracking()
                                                          .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                                           && x.Wallet.WalletID == walletId && x.TransactionType.TransactionTypeID == 2)
                                                          .OrderByDescending(desc => desc.TransactionDate)
                                                          .ToListAsync();
                    }
                }

                //else //if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                //{
                //    userDetails = await Context.SYS_Users
                //            .Include(u => u.Role)
                //            .Include(u => u.Wallet)
                //            .Where(p => p.UserID == userId)
                //            .FirstOrDefaultAsync();

                //    if (userDetails.BankAccountCode != null)
                //    {
                //        transData = await Context.EGS_WalletTransaction
                //            .Include(tTyp => tTyp.TransactionType)
                //            .Include(mTyp => mTyp.TransactionMethod)
                //            .Include(pItem => pItem.ProductItem)
                //            .Include(usr => usr.Wallet)
                //            .Include(wuser => wuser.Wallet.User)
                //            //.ThenInclude(u => u.User)
                //            .AsNoTracking()
                //            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                //            && x.TransactionType.TransactionTypeID == 2 /*&& x.CreatedBy == userId*/)
                //            .OrderByDescending(desc => desc.TransactionDate)
                //            .ToListAsync();
                //    }
                //    else
                //    {
                //        transData = await Context.EGS_WalletTransaction
                //                .Include(tTyp => tTyp.TransactionType)
                //                .Include(mTyp => mTyp.TransactionMethod)
                //                .Include(pItem => pItem.ProductItem)
                //                .Include(usr => usr.Wallet)
                //                .Include(wuser => wuser.Wallet.User)
                //                //.ThenInclude(u => u.User)
                //                .AsNoTracking()
                //                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                //                && x.TransactionType.TransactionTypeID == 2/* && x.Wallet.WalletID == userDetails.Wallet.WalletID*/)
                //                .OrderByDescending(desc => desc.TransactionDate)
                //                .ToListAsync();
                //    }
                //}
            }
            else if (queryStatus.ToUpper() == "CREDIT")
            {
                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN") || roleName.ToUpper().Contains("OPERATIONS") || roleName.ToUpper().Contains("CUSTOMER RECONCILATION"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                            .Include(u => u.Role)
                            .Include(u => u.Wallet)
                            .Where(p => p.UserID == userId)
                            .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.TransactionType.TransactionTypeID == 1 && x.CreatedBy == userId)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null)
                        {
                            if (walletId != 0)
                            {
                                transData = await Context.EGS_WalletTransaction
                                                                   .Include(tTyp => tTyp.TransactionType)
                                                                   .Include(mTyp => mTyp.TransactionMethod)
                                                                   //.Include(pItem => pItem.ProductItem)
                                                                   .Include(usr => usr.Wallet)
                                                                   .Include(wuser => wuser.Wallet.User)
                                                                   //.ThenInclude(u => u.User)
                                                                   .AsNoTracking()
                                                                   .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                                                   && x.TransactionType.TransactionTypeID == 1 && x.Wallet.WalletID == walletId)
                                                                   .OrderByDescending(desc => desc.TransactionDate)
                                                                   .ToListAsync();
                            }
                            else if (walletId == 0)
                            {
                                transData = await Context.EGS_WalletTransaction
                                     .Include(tTyp => tTyp.TransactionType)
                                      .Include(mTyp => mTyp.TransactionMethod)
                                     //.Include(pItem => pItem.ProductItem)
                                     .Include(usr => usr.Wallet)
                                     .Include(wuser => wuser.Wallet.User)
                                     .AsNoTracking()
                                       .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                      && x.TransactionType.TransactionTypeID == 1)
                                       .OrderByDescending(desc => desc.TransactionDate)
                                      .ToListAsync();
                            }

                        }
                        //&& x.Wallet.WalletID == userDetails.Wallet.WalletID
                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                                && x.TransactionType.TransactionTypeID == 1)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                else if (roleName.ToUpper().Contains("CUSTOMER CARE"))
                {
                    if (userId != 0)
                    {
                        userDetails = await Context.SYS_Users
                            .Include(u => u.Role)
                            .Include(u => u.Wallet)
                            .Where(p => p.UserID == userId)
                            .FirstOrDefaultAsync();

                        if (userDetails.BankAccountCode != null)
                        {
                            transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.TransactionType.TransactionTypeID == 1
                                && x.CreatedBy == userId)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                        }
                        else if (userDetails.BankAccountCode == null && userDetails.Wallet.WalletID > 0)
                        {
                            transData = await Context.EGS_WalletTransaction
                                    .Include(tTyp => tTyp.TransactionType)
                                    .Include(mTyp => mTyp.TransactionMethod)
                                    .Include(pItem => pItem.ProductItem)
                                    .Include(usr => usr.Wallet)
                                    .Include(wuser => wuser.Wallet.User)
                                    //.ThenInclude(u => u.User)
                                    .AsNoTracking()
                                    .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.TransactionType.TransactionTypeID == 1 && x.Wallet.WalletID == userDetails.Wallet.WalletID)
                                    .OrderByDescending(desc => desc.TransactionDate)
                                    .ToListAsync();
                        }

                    }
                    else
                    {
                        transData = await Context.EGS_WalletTransaction
                                .Include(tTyp => tTyp.TransactionType)
                                .Include(mTyp => mTyp.TransactionMethod)
                                .Include(pItem => pItem.ProductItem)
                                .Include(usr => usr.Wallet)
                                .Include(wuser => wuser.Wallet.User)
                                //.ThenInclude(u => u.User)
                                .AsNoTracking()
                                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date && x.TransactionType.TransactionTypeID == 1)
                                .OrderByDescending(desc => desc.TransactionDate)
                                .ToListAsync();
                    }

                }
                if ((roleName.ToUpper().Contains("AGGREGATOR") || roleName.ToUpper().Contains("AGENT")))
                {
                    if (walletId != 0 && queryStatus.ToUpper() == "CREDIT")
                    {
                        transData = await Context.EGS_WalletTransaction
                           .Include(tTyp => tTyp.TransactionType)
                           .Include(mTyp => mTyp.TransactionMethod)
                           .Include(pItem => pItem.ProductItem)
                           .Include(usr => usr.Wallet)
                           .Include(wuser => wuser.Wallet.User)
                           //.ThenInclude(u => u.User)
                           .AsNoTracking()
                           .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                            && x.Wallet.WalletID == walletId && x.TransactionType.TransactionTypeID == 1)
                           .OrderByDescending(desc => desc.TransactionDate)
                           .ToListAsync();
                    }
                }
                //else //if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                //{
                //    userDetails = await Context.SYS_Users
                //            .Include(u => u.Role)
                //            .Include(u => u.Wallet)
                //            .Where(p => p.UserID == userId)
                //            .FirstOrDefaultAsync();

                //    if (userDetails.BankAccountCode != null)
                //    {
                //        transData = await Context.EGS_WalletTransaction
                //            .Include(tTyp => tTyp.TransactionType)
                //            .Include(mTyp => mTyp.TransactionMethod)
                //            .Include(pItem => pItem.ProductItem)
                //            .Include(usr => usr.Wallet)
                //            .Include(wuser => wuser.Wallet.User)
                //            //.ThenInclude(u => u.User)
                //            .AsNoTracking()
                //            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                //            && x.TransactionType.TransactionTypeID == 1 && x.CreatedBy == userId)
                //            .OrderByDescending(desc => desc.TransactionDate)
                //            .ToListAsync();
                //    }
                //    else
                //    {
                //        transData = await Context.EGS_WalletTransaction
                //                .Include(tTyp => tTyp.TransactionType)
                //                .Include(mTyp => mTyp.TransactionMethod)
                //                .Include(pItem => pItem.ProductItem)
                //                .Include(usr => usr.Wallet)
                //                .Include(wuser => wuser.Wallet.User)
                //                //.ThenInclude(u => u.User)
                //                .AsNoTracking()
                //                .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                //                && x.TransactionType.TransactionTypeID == 1 /*&& x.Wallet.WalletID == userDetails.Wallet.WalletID*/)
                //                .OrderByDescending(desc => desc.TransactionDate)
                //                .ToListAsync();
                //    }
                //}
            }

            return transData;
        }

        public async Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByDateRangeAndUserID(DateTime start, DateTime end, int ID)
        {
            IEnumerable<EgsWalletTransaction> transData = null;
            try
            {
                transData = await Context.EGS_WalletTransaction
                            //.Include(inv => inv.Invoices)
                            .Include(tTyp => tTyp.TransactionType)
                               .Include(mTyp => mTyp.TransactionMethod)
                            .Include(pItem => pItem.ProductItem)
                            .Include(usr => usr.Wallet)
                            .Include(wuser => wuser.Wallet.User)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date >= start.Date && x.TransactionDate.Date <= end.Date
                            && (x.Invoices.PaymentStatus == true || x.Amount > 0) && x.Wallet.WalletID == ID)//x.Wallet.User.UserID == ID)
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();

                return transData;
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
                transData = await Context.EGS_WalletTransaction.Where(x => x.TransactionDate >= start && x.TransactionDate <= end && x.Invoices.PaymentStatus == true)

                            .AsNoTracking()
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsWalletTransaction>> GetTransactionsByIDAndValue(int ID, int Value)
        {
            IEnumerable<EgsWalletTransaction> transData = null;
            try
            {
                transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .AsNoTracking()
                            .Where(x => (x.Invoices.PaymentStatus == true || x.Amount > 0) && x.Wallet.User.UserID == ID)
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .Take(Value)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsWalletTransaction>> GetWalletTransaction(int userId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus)
        {
            IEnumerable<EgsWalletTransaction> transData = null;

            if (queryStatus.ToUpper() == "ALL")
            {
                if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
                {
                    transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .Include(pitm => pitm.ProductItem)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                            && (x.Invoices.PaymentStatus == true || x.Amount > 0) && x.Wallet.User.UserID == userId)
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();
                }

                if (roleName.ToUpper().Contains("MERCHANT"))
                {
                    transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .Include(pitm => pitm.ProductItem)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                            && (x.Invoices.PaymentStatus == true || x.Amount > 0) && x.Wallet.User.UserID == userId)
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();
                    //await Context.EGS_WalletTransaction
                    //        .Include(prd => prd.Product)
                    //        .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId)
                    //        .AsNoTracking()
                    //        .ToListAsync();
                }

                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                {
                    transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .Include(pitm => pitm.ProductItem)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date == startPeriod.Date && (x.Invoices.PaymentStatus == true || x.Amount > 0))
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();


                    //await Context.EGS_WalletTransaction.Where(p => (p.PaymentStatus == true || p.Amount > 0) && p.DateCreated.Date == DateTime.Now.Date)
                    //          .AsNoTracking()
                    //          .ToListAsync();
                }

            }

            //if (queryStatus.ToUpper() == "COMPLETED")
            //{
            //    if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
            //    {
            //        transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentDate.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == true)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("MERCHANT"))
            //    {
            //        transData = await Context.EGS_WalletTransaction
            //                    .Include(prd => prd.Product)
            //                    .Where(p => p.PaymentDate.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == true)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
            //    {
            //        //transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentDate.Date == startPeriod.Date && p.PaymentStatus == true)
            //        //            .AsNoTracking()
            //        //            .ToListAsync();

            //        transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentStatus == true && p.DateCreated.Date == DateTime.Now.Date)
            //                   .AsNoTracking()
            //                   .ToListAsync();
            //    }

            //}

            //if (queryStatus.ToUpper() == "UNCOMPLETED")
            //{
            //    if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
            //    {
            //        transData = await Context.EGS_WalletTransaction.Where(p => p.DateCreated.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == false)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("MERCHANT"))
            //    {
            //        transData = await Context.EGS_WalletTransaction
            //                    .Include(prd => prd.Product)
            //                    .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == false)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
            //    {
            //        //transData = await Context.EGS_WalletTransaction.Where(p => p.DateCreated.Date == startPeriod.Date && p.PaymentStatus == false)
            //        //            .AsNoTracking()
            //        //            .ToListAsync();
            //        transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentStatus == false && p.DateCreated.Date == DateTime.Now.Date)
            //                   .AsNoTracking()
            //                   .ToListAsync();

            //    }

            //}

            return transData;
        }

        public async Task<IEnumerable<EgsWalletTransaction>> GetTodayWalletTransaction(int userId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus)
        {
            IEnumerable<EgsWalletTransaction> transData = null;

            if (queryStatus.ToUpper() == "ALL")
            {
                if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
                {
                    transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .Include(pitm => pitm.ProductItem)
                            //.Include(inv => inv.Invoices)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date //&& (x.Invoices.PaymentStatus == true || x.Amount > 0) 
                                && x.Wallet.User.UserID == userId)
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();
                }

                if (roleName.ToUpper().Contains("MERCHANT"))
                {
                    transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .Include(pitm => pitm.ProductItem)
                            //.ThenInclude(p => p.Product)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date
                            && (x.Wallet.User.UserID == userId || x.ProductItem.Product.Merchant.MerchantID == userId))
                            //&& (x.Invoices.PaymentStatus == true || x.Amount > 0) && x.ProductItem.Product.Merchant.MerchantID == userId)
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();
                }

                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                {
                    transData = await Context.EGS_WalletTransaction
                            .Include(tTyp => tTyp.TransactionType)
                            .Include(usr => usr.Wallet)
                            .Include(pitm => pitm.ProductItem)
                            .AsNoTracking()
                            .Where(x => x.TransactionDate.Date >= startPeriod.Date && x.TransactionDate.Date <= endPeriod.Date)
                            //&& (x.Invoices.PaymentStatus == true || x.Amount > 0))
                            .OrderByDescending(desc => desc.TransactionDate.Date)
                            .ToListAsync();


                    //await Context.EGS_WalletTransaction.Where(p => (p.PaymentStatus == true || p.Amount > 0) && p.DateCreated.Date == DateTime.Now.Date)
                    //          .AsNoTracking()
                    //          .ToListAsync();
                }

            }

            //if (queryStatus.ToUpper() == "COMPLETED")
            //{
            //    if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
            //    {
            //        transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentDate.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == true)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("MERCHANT"))
            //    {
            //        transData = await Context.EGS_WalletTransaction
            //                    .Include(prd => prd.Product)
            //                    .Where(p => p.PaymentDate.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == true)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
            //    {
            //        //transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentDate.Date == startPeriod.Date && p.PaymentStatus == true)
            //        //            .AsNoTracking()
            //        //            .ToListAsync();

            //        transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentStatus == true && p.DateCreated.Date == DateTime.Now.Date)
            //                   .AsNoTracking()
            //                   .ToListAsync();
            //    }

            //}

            //if (queryStatus.ToUpper() == "UNCOMPLETED")
            //{
            //    if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
            //    {
            //        transData = await Context.EGS_WalletTransaction.Where(p => p.DateCreated.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == false)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("MERCHANT"))
            //    {
            //        transData = await Context.EGS_WalletTransaction
            //                    .Include(prd => prd.Product)
            //                    .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == false)
            //                    .AsNoTracking()
            //                    .ToListAsync();
            //    }

            //    if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
            //    {
            //        //transData = await Context.EGS_WalletTransaction.Where(p => p.DateCreated.Date == startPeriod.Date && p.PaymentStatus == false)
            //        //            .AsNoTracking()
            //        //            .ToListAsync();
            //        transData = await Context.EGS_WalletTransaction.Where(p => p.PaymentStatus == false && p.DateCreated.Date == DateTime.Now.Date)
            //                   .AsNoTracking()
            //                   .ToListAsync();

            //    }

            //}

            return transData;
        }

    }
}
