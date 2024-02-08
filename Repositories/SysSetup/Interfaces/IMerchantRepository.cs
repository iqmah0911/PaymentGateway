//using PaymentGateway.Repositories;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IMerchantRepository : IRepository<EgsMerchant>
    {
        List<EgsMerchant> GetAllForDropdown();
        List<SysUsers> GetAllMerchantsExcludingid(int merchantid);
        List<SysUsers> GetAllMerchants();
        Task<SysUsers> GetMerchantByCode(string merchantcode);
        Task<SysUsers> GetMerchantsById(int Id);
        Task UpdateMerchantAsync(SysUsers entity);
        Task UpdateMerchant(EgsMerchant entity);
        Task<List<SysUsers>> GetAllTMerchants();
        Task<EgsMerchant> GetMerchantsFromId(int Id);
    }
}
