using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.Reports.Interfaces
{
    public interface IMerchantReportRepository
    {
        Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRange(DateTime start, DateTime end);
        Task<IEnumerable<EgsSettlementSummary>> GetMerchantSettlement(int id);
        Task<IEnumerable<EgsSettlementSummary>> GetSettlementByDateRange(DateTime start, DateTime end);
        Task<IEnumerable<EgsSettlementLog>> GetMerchantSettlementDetails(int id);
        Task<IEnumerable<EgsSettlementLog>> GetSettlementDetailsByDateRange(DateTime start, DateTime end);
    }
}
