using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IMerchantMappingRepository : IRepository<SysMerchantMapping>
    {
        Task<List<SysMerchantMapping>> FetchSubMerchantByMerchantID(int id);
        Task<List<SysMerchantMapping>> FetchAllSubMerchants();
        Task<List<SysUsers>> GetMerchants();
        Task<List<SysMerchantMapping>> FetchMappedSubMerchants();
    }
}
