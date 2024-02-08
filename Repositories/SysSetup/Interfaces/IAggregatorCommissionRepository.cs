using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IAggregatorCommissionRepository : IRepository<EgsAggregatorCommission>
    {
        Task<HoldDisplayAggViewModel> GetAggregatorMonthlyCommision(int aggid);
    }
}
