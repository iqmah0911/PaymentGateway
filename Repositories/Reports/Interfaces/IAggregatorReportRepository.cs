using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.Reports.Interfaces
{
    public interface IAggregatorReportRepository : IRepository<EgsSales>
    {
        Task<IEnumerable<EgsSales>> GetSalesByDateRange(DateTime start, DateTime end, int id);
        Task<IEnumerable<EgsSales>> GetAllSalesByDateRange(DateTime start, DateTime end, List<string> Id);

    }
}
