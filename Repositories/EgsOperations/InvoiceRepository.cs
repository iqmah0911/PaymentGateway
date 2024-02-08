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
    public class InvoiceRepository : Repository<EgsInvoice>, IInvoiceRepository
    {
        ApplicationDBContext _context;

        public InvoiceRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateInvoice(EgsInvoice entity)
        {
            try
            {
                //Context.Entry(entity).State = EntityState.Modified;
                //Context.SaveChanges();
                 
               
                //Context.Entry(entity).State = EntityState.Modified;
                //Context.EGS_Invoice.Update(entity);
                Context.SaveChanges();

                //Context.Update(entity);
                //Context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {

            }
        }


        public async Task<EgsInvoice> GetInvoice(int id)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.InvoiceID == id)
                            .Include(x => x.Product)
                            .Include(x => x.Bank) 
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByID(int id)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.CreatedBy == id)
                            .Include(x => x.Product)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .ToListAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByMerchantID(int id) // merchantid
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice//.Where(p => p.CreatedBy == id)
                            .Include(x => x.Product)
                            .Include(x => x.Bank)
                            .Where( mer => mer.Product.Merchant.MerchantID == id)
                            .AsNoTracking()
                            .ToListAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsWalletTransaction>> GetWalletBalance(int id)
        {
            try
            {
                var balance = await Context.EGS_WalletTransaction
                            .Include(x => x.Wallet)
                            //.ThenInclude(x => x.User)
                            .Include(x => x.TransactionType)
                            .AsNoTracking()
                            .Where(p => p.Wallet.WalletID == id)
                            .ToListAsync();

                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsWallet> GetNWalletBalance(int id)
        {
            try
            {
                var balance = await Context.EGS_Wallet
                            .AsNoTracking()
                            .Where(p => p.WalletID == id).FirstOrDefaultAsync(); 

                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<List<EgsWalletTransaction>> GetWalletBalance(int id,DateTime date)
        {
            try
            {
                var balance = await Context.EGS_WalletTransaction
                            .Include(x => x.Wallet)
                            //.ThenInclude(x => x.User)
                            .Include(x => x.TransactionType)
                            .AsNoTracking()
                            .Where(p => p.Wallet.WalletID==id && p.TransactionDate <= date)
                            .ToListAsync();

                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsInvoice>> GetUnPaidInvoice(int id)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.CreatedBy == id && p.PaymentStatus == false)
                            .Include(x => x.Product)
                            .Include(x => x.Bank)
                            .AsNoTracking()
                            .ToListAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsInvoice>> GetInvoices()
        {
            try
            {
                var invoices = await Context.EGS_Invoice
                            .AsNoTracking()
                            .ToListAsync();

                return invoices;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsInvoice> GetInvoicesReferenceNo(string refNo)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.ReferenceNo == refNo || p.AlternateReferenceNo == refNo)
                            .Include(x => x.Product)
                            .Include(x => x.Bank)
                            .SingleOrDefaultAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EgsInvoice> GetInvoicesByAlternateRef(string refNo)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.AlternateReferenceNo == refNo ||p.CustomerAlternateRef==refNo && p.PaymentStatus==true)
                            .Include(x => x.Product)
                            .Include(x => x.Bank)
                            .FirstOrDefaultAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsInvoice>> GetInvoicesReference(string refNo)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.ReferenceNo == refNo || p.AlternateReferenceNo == refNo)
                              .AsNoTracking()
                            .ToListAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePaidInvoice(EgsInvoice entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                //Context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }


        public async Task<bool> UpdateInvc(EgsInvoice entity)
        {
            try
            {
                var entry =  Context.EGS_Invoice.First(e => e.InvoiceID == entity.InvoiceID);
                Context.Entry(entry).CurrentValues.SetValues(entity);
                //Context.SaveChanges();
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                // handle correct exception
                // log error
                return false;
            }
        }




        public async Task<EgsInvoice> GetReferenceNo(string prct)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.ReferenceNo == prct || p.AlternateReferenceNo == prct)
                            .Include(x => x.Product)
                            //.Include(b => b.Bank)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsInvoice> UpdateReferenceNo(string refNo)
        {
            try
            {
                var invoiceDetails = await Context.EGS_Invoice.Where(p => p.ReferenceNo == refNo && p.PaymentStatus == false)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsInvoice>> GetTransaction(int userId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus)
        {
            IEnumerable<EgsInvoice> transData = null;

            if (queryStatus.ToUpper() == "ALL")
            {
                if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL") || roleName.ToUpper().Contains("SUBAGENT"))
                {
                    transData = await Context.EGS_Invoice
                        //.Include(walTrs => walTrs.WalletTransactions)
                        .Where(p => p.DateCreated.Date <= startPeriod.Date && p.DateCreated.Date <= endPeriod.Date && p.CreatedBy == userId)
                                .AsNoTracking()
                                .ToListAsync();

                    //var firstNames = new[]
                    //{
                    //    new { ID = 1, Name = "John" },
                    //    new { ID = 2, Name = "Sue" },
                    //};
                    //var lastNames = new[]
                    //{
                    //    new { ID = 1, Name = "Doe" },
                    //    new { ID = 3, Name = "Smith" },
                    //};
                    //var leftOuterJoin =
                    //    from first in firstNames
                    //    join last in lastNames on first.ID equals last.ID into temp
                    //    from last in temp.DefaultIfEmpty()
                    //    select new
                    //    {
                    //        first.ID,
                    //        FirstName = first.Name,
                    //        LastName = last?.Name,
                    //    };
                    //var rightOuterJoin =
                    //    from last in lastNames
                    //    join first in firstNames on last.ID equals first.ID into temp
                    //    from first in temp.DefaultIfEmpty()
                    //    select new
                    //    {
                    //        last.ID,
                    //        FirstName = first?.Name,
                    //        LastName = last.Name,
                    //    };
                    //var fullOuterJoin = leftOuterJoin.Union(rightOuterJoin);


                }

                if (roleName.ToUpper().Contains("MERCHANT"))
                {
                    transData = await Context.EGS_Invoice
                                .Include(prd => prd.Product)
                                .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId)
                                .AsNoTracking()
                                .ToListAsync();
                }

                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                {
                    transData = await Context.EGS_Invoice.Where(p => (p.PaymentStatus== true || p.Amount > 0) && p.DateCreated.Date == startPeriod.Date)
                              .AsNoTracking()
                              .ToListAsync();
                }

            }

            if (queryStatus.ToUpper() == "COMPLETED")
            {
                if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL") || roleName.ToUpper().Contains("SUBAGENT"))
                {
                    transData = await Context.EGS_Invoice.Where(p => p.PaymentDate.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == true)
                                .AsNoTracking()
                                .ToListAsync();
                }

                if (roleName.ToUpper().Contains("MERCHANT"))
                {
                    transData = await Context.EGS_Invoice
                                .Include(prd => prd.Product)
                                .Where(p => p.PaymentDate.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == true)
                                .AsNoTracking()
                                .ToListAsync();
                }

                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                {

                    transData = await Context.EGS_Invoice.Where(p => (p.PaymentStatus == true) && p.DateCreated.Date == startPeriod.Date)
                               .AsNoTracking()
                               .ToListAsync();
                }

            }

            if (queryStatus.ToUpper() == "UNCOMPLETED")
            {
                if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL") || roleName.ToUpper().Contains("SUBAGENT"))
                {
                    transData = await Context.EGS_Invoice.Where(p => p.DateCreated.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == false)
                                .AsNoTracking()
                                .ToListAsync();
                }

                if (roleName.ToUpper().Contains("MERCHANT"))
                {
                    transData = await Context.EGS_Invoice
                                .Include(prd => prd.Product)
                                .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == false)
                                .AsNoTracking()
                                .ToListAsync();
                }

                if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
                {
                    //transData = await Context.EGS_Invoice.Where(p => p.DateCreated.Date == startPeriod.Date && p.PaymentStatus == false)
                    //            .AsNoTracking()
                    //            .ToListAsync();
                    transData = await Context.EGS_Invoice.Where(p => p.PaymentStatus == false && p.DateCreated.Date == startPeriod.Date)
                               .AsNoTracking()
                               .ToListAsync();

                }

            }

            return transData;
        }


        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRange(DateTime start, DateTime end)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x=>x.PaymentDate>= start && x.PaymentDate<= end)
                            .AsNoTracking()
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRangeAndProductId(DateTime start, DateTime end, int ProductId)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x => x.PaymentDate >= start && x.PaymentDate <= end && x.Product.ProductID==ProductId)
                            .AsNoTracking()
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
