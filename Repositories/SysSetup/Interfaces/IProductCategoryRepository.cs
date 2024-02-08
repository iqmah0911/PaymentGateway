//using PaymentGateway.Repositories;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IProductCategoryRepository : IRepository<EgsProductCategory>
    {
        Task<List<EgsProductCategory>> GetAllProductCategory();
        List<EgsProductCategory> GetAllForDropdown();
        Task<EgsProductCategory> GetProductCategory(int id);
        List<EgsProductCategory> GetAllForDropdownList();
        Task<EgsProductCategory> GetProductCategoryByName(string CategoryName);
    }
}
