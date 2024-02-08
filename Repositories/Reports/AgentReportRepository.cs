using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.Reports.Interfaces
{
    public class AgentReportRepository : Repository<EgsInvoice>, IAgentReportRepository
    {
        ApplicationDBContext _context;

        public AgentReportRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRange(DateTime start, DateTime end,int id)
        {
            IEnumerable<EgsInvoice> transData = null;
            try 
            {
                transData = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end &&  x.CreatedBy == id)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByID(int id)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x => x.PaymentStatus == true && x.CreatedBy == id)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDate(DateTime start, DateTime end)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end)// && x.PaymentStatus == true
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByReferenceNo(string ReferenceNo)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x => x.ReferenceNo == ReferenceNo || x.PaymentReference== ReferenceNo 
                || x.AlternateReferenceNo == ReferenceNo)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDateProduct(DateTime start, DateTime end, int prodid)
        {
            try
            {
                List<EgsInvoice> rptInvDetails = new List<EgsInvoice>();

                if (prodid == 0)
                {
                     rptInvDetails = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end)
                                .Include(p => p.Product)
                                .Include(p => p.Bank)
                                .ToListAsync();
                }
                else
                {
                     rptInvDetails = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.Product.ProductID == prodid )
                                .Include(p => p.Product)
                                .Include(x => x.Bank)
                                .ToListAsync();
                }

                return rptInvDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDateProductItem(DateTime start, DateTime end, int prodid)
        {
            try
            {
                List<EgsInvoice> rptInvDetails = new List<EgsInvoice>();

                if (prodid == 0)
                {
                    rptInvDetails = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end)
                               .Include(p => p.Product)
                               .Include(p => p.Bank)
                               .ToListAsync();
                }
                else
                {
                    rptInvDetails = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.ProductItemID == prodid)
                               .Include(p => p.Product)
                               .Include(x => x.Bank)
                               .ToListAsync();
                }

                return rptInvDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByProductID(DateTime start, DateTime end, int id)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.Product.ProductID == id)
                    .Include(p=>p.Product)
                    .Include(pr=>pr.Product.ProductItems)
                    .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByProductDateRange(DateTime start, DateTime end, int prodid, int uId)
        {
            try
            {
                List<EgsInvoice> rptInvDetails = new List<EgsInvoice>();
                //IEnumerable<EgsInvoice> transData = null;
                if (prodid == 0)
                {
                    rptInvDetails = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.CreatedBy == uId)
                               .Include(x => x.Product)
                               .Include(x => x.Bank)
                               .ToListAsync();
                }
                else
                {
                    rptInvDetails = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.Product.ProductID == prodid && x.CreatedBy == uId)
                               .Include(p => p.Product)
                               .Include(x => x.Bank)
                               .ToListAsync();
                }

                return rptInvDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        //public async Task<IEnumerable<EgsInvoice>> GetTransaction(int userId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus)
        //{
        //    IEnumerable<EgsInvoice> transData = null;

        //    if (queryStatus.ToUpper() == "ALL")
        //    {
        //        if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
        //        {
        //            transData = await Context.EGS_Invoice.Where(p => p.DateCreated.Date.Date == startPeriod.Date.Date && p.CreatedBy == userId)
        //                        .AsNoTracking()
        //                        .ToListAsync();
        //        }

        //        if (roleName.ToUpper().Contains("MERCHANT"))
        //        {
        //            transData = await Context.EGS_Invoice
        //                        .Include(prd => prd.Product)
        //                        .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId)
        //                        .AsNoTracking()
        //                        .ToListAsync();
        //        }

        //        if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
        //        {
        //            //transData = await Context.EGS_Invoice.Where(p => p.DateCreated.Date == startPeriod.Date)
        //            //            .AsNoTracking()
        //            //            .ToListAsync();

        //            transData = await Context.EGS_Invoice.Where(p => p.PaymentStatus == true && p.DateCreated.Date == DateTime.Now.Date)
        //                      .AsNoTracking()
        //                      .ToListAsync();
        //        }

        //    }

        //    if (queryStatus.ToUpper() == "COMPLETED")
        //    {
        //        if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
        //        {
        //            transData = await Context.EGS_Invoice.Where(p => p.PaymentDate.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == true)
        //                        .AsNoTracking()
        //                        .ToListAsync();
        //        }

        //        if (roleName.ToUpper().Contains("MERCHANT"))
        //        {
        //            transData = await Context.EGS_Invoice
        //                        .Include(prd => prd.Product)
        //                        .Where(p => p.PaymentDate.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == true)
        //                        .AsNoTracking()
        //                        .ToListAsync();
        //        }

        //        if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
        //        {
        //            //transData = await Context.EGS_Invoice.Where(p => p.PaymentDate.Date == startPeriod.Date && p.PaymentStatus == true)
        //            //            .AsNoTracking()
        //            //            .ToListAsync();

        //            transData = await Context.EGS_Invoice.Where(p => p.PaymentStatus == true && p.DateCreated.Date == DateTime.Now.Date)
        //                       .AsNoTracking()
        //                       .ToListAsync();
        //        }

        //    }

        //    if (queryStatus.ToUpper() == "UNCOMPLETED")
        //    {
        //        if (roleName.ToUpper().Contains("AGENT") || roleName.ToUpper().Contains("GENERAL"))
        //        {
        //            transData = await Context.EGS_Invoice.Where(p => p.DateCreated.Date == startPeriod.Date && p.CreatedBy == userId && p.PaymentStatus == false)
        //                        .AsNoTracking()
        //                        .ToListAsync();
        //        }

        //        if (roleName.ToUpper().Contains("MERCHANT"))
        //        {
        //            transData = await Context.EGS_Invoice
        //                        .Include(prd => prd.Product)
        //                        .Where(p => p.DateCreated.Date == startPeriod.Date && p.Product.Merchant.MerchantID == userId && p.PaymentStatus == false)
        //                        .AsNoTracking()
        //                        .ToListAsync();
        //        }

        //        if (roleName.ToUpper().Contains("ADMINISTRATOR") || roleName.ToUpper().Contains("SUPER ADMIN"))
        //        {
        //            //transData = await Context.EGS_Invoice.Where(p => p.DateCreated.Date == startPeriod.Date && p.PaymentStatus == false)
        //            //            .AsNoTracking()
        //            //            .ToListAsync();
        //            transData = await Context.EGS_Invoice.Where(p => p.PaymentStatus == false && p.DateCreated.Date == DateTime.Now.Date)
        //                       .AsNoTracking()
        //                       .ToListAsync();

        //        }

        //    }

        //    return transData;
        //}



    }
}
