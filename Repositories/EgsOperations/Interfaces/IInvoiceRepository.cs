using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations.Interfaces
{
    public interface IInvoiceRepository: IRepository<EgsInvoice>
    {
        Task<EgsInvoice> GetInvoice(int id);
        Task<List<EgsInvoice>> GetInvoices();
        Task<EgsInvoice> GetInvoicesReferenceNo(string refNo);
        Task<EgsInvoice> GetReferenceNo(string prct);
        Task<IEnumerable<EgsInvoice>> GetUnPaidInvoice(int id);
        Task<List<EgsWalletTransaction>> GetWalletBalance(int id);
        Task<EgsInvoice> UpdateReferenceNo(string refNo);
        void UpdateInvoice(EgsInvoice entity);
        //Task<EgsInvoice> UpdateInvoice(EgsInvoice entity);
        Task<IEnumerable<EgsInvoice>> GetTransaction(int userId, DateTime startPeriod, DateTime endPeriod, string roleName, string queryStatus);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRange(DateTime start, DateTime end);
        Task<EgsWallet> GetNWalletBalance(int id);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByID(int id);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByMerchantID(int id);

        Task<List<EgsInvoice>> GetInvoicesReference(string refNo);
        Task<EgsInvoice> GetInvoicesByAlternateRef(string refNo);
        Task<List<EgsWalletTransaction>> GetWalletBalance(int id, DateTime date);
        void UpdatePaidInvoice(EgsInvoice entity);

        //bool UpdateInvc(EgsInvoice entity);

        Task<bool> UpdateInvc(EgsInvoice entity);
        Task<IEnumerable<EgsInvoice>> GetInvoicesByDateRangeAndProductId(DateTime start, DateTime end, int ProductId);
    }

}
