using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.Reports.Interfaces
{
    public interface IAgentReportRepository : IRepository<EgsInvoice>
    {

        Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRange(DateTime start, DateTime end,int id);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByProductID(DateTime start, DateTime end, int id);

        Task<IEnumerable<EgsInvoice>> GetInvoicesByDate(DateTime start, DateTime end);

        Task<IEnumerable<EgsInvoice>> GetInvoicesByReferenceNo(string ReferenceNo);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByDateProduct(DateTime start, DateTime end, int prodid);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByID(int id);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByProductDateRange(DateTime start, DateTime end, int prodid, int uId);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByDateProductItem(DateTime start, DateTime end, int prodid);
    }
}
