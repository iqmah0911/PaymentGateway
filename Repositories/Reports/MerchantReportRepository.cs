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
    public class MerchantReportRepository : Repository<EgsInvoice>, IMerchantReportRepository
    {
        ApplicationDBContext _context;

        public MerchantReportRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRange(DateTime start, DateTime end)
        {
            IEnumerable<EgsInvoice> transData = null;
            try
            {
                transData = await Context.EGS_Invoice.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.PaymentStatus == true)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<IEnumerable<EgsSettlementSummary>> GetMerchantSettlement(int id)
        {
            IEnumerable<EgsSettlementSummary> transData = null;
            try
            {
                transData = await Context.EGS_SettlementSummary.Where(x => x.Merchant.MerchantID >= id)
                             .Include(r=>r.Merchant)
                              .Include(r => r.Bank)
                              .Include(r=>r.Product)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsSettlementSummary>> GetSettlementByDateRange(DateTime start, DateTime end)
        {
            IEnumerable<EgsSettlementSummary> transData = null;
            try
            {
                transData = await Context.EGS_SettlementSummary.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end)
                            .Include(r => r.Merchant)
                              .Include(r => r.Bank)
                              .Include(r=>r.Product)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsSettlementLog>> GetMerchantSettlementDetails(int id)
        {
            IEnumerable<EgsSettlementLog> transData = null;
            try
            {
                transData = await Context.EGS_SettlementLog.Where(x => x.Merchant.MerchantID >= id)
                            .Include(r=>r.Product)
                            .Include(r => r.ProductItem)
                            .Include(r => r.SettlementMode)
                             .Include(r => r.SettlementType)
                            .Include(r => r.Merchant)
                            .Include(r=>r.Bank)
                            .ToListAsync();

                return transData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsSettlementLog>> GetSettlementDetailsByDateRange(DateTime start, DateTime end)
        {
            IEnumerable<EgsSettlementLog> transData = null;
            try
            {
                transData = await Context.EGS_SettlementLog.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end)
                             .Include(r => r.Product)
                            .Include(r => r.ProductItem)
                            .Include(r => r.SettlementMode)
                             .Include(r => r.SettlementType)
                            .Include(r => r.Merchant)
                            .Include(r => r.Bank)
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


