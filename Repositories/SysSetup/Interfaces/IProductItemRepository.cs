using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IProductItemRepository : IRepository<EgsProductItem>
    {
        Task<EgsProductItem> GetProductItems(int id);
        Task<List<EgsProductItem>> GetProductItemCategories();
       //Task<IEnumerable<EgsProductItemViewModel>> GetProductitemCategory();
        Task<EgsProductItem> GetProductItemCategoriesByCat(string itemCat);
        Task<List<EgsProductItem>> GetAllProductItems();
        Task<EgsProductItem> GetProductItemsByProdId(int id);
        Task<List<EgsProductItem>> GetItemsByProductID(int id);
        Task<EgsProductItem> GetProductItem(int id);
        Task<EgsProductItem> GetProductItemByItemCode(string itemCode);
        Task<EgsProductItem> GetSingleProductItem(string itemCode, int productId);
        Task<EgsProductItem> GetProducttransactionfee(int prdid); 
        Task<List<EgsProductItem>> GetProductProcessingItems();
        Task<EgsProductItem> GetSingleProductByItem(int pItemId);
        Task<EgsProductItem> GetProductItemByItemName(string itemName);
    }
}
