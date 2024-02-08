using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IProductItemRateRepository : IRepository<EgsProductItemRate>
    {
        Task<EgsProductItemRate> GetProductItemRates(int id);
        Task<List<EgsProductItemRate>> GetAllProductItemRates();
        Task<EgsProductItemRate> GetProductItemsByProdItemId(int id);
        Task<List<EgsProductItemRate>> GetAllProductItemRatesByItemId(int id);
        Task<List<EgsProductItemRate>> GetItemRateByProductID(int id);
        Task<EgsProductItemRate> GetItemRateByProductItemID(int id);
        Task<EgsProductItemRate> GetItemRateByProductItemIDAPI(int id);
    }
}
