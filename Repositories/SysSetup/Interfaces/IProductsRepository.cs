//using PaymentGateway.Repositories;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup.Interfaces
{
    public interface IProductsRepository : IRepository<EgsProduct>
    {
        Task<EgsProduct> GetProducts(int id);
        Task<List<EgsProduct>> GetAllProducts();
        Task<EgsProduct> GetProductCategoryById(int id);
        Task<List<EgsProduct>> GetProductsByMerchantId(int id);
        Task<EgsProductItem> GetProductExtension(int id);
        Task<EgsProduct> GetProductInvoiceModeById(int id);
        Task<List<EgsProduct>> GetProductList();
        Task<IEnumerable<EgsProductViewModel>> GetProductNasarawaByCategoryID();
        Task<EgsProduct> GetTransactionfeebyProductCode(string code);
        Task<EgsProduct> GetProductByName(string name);
        Task<IEnumerable<EgsProductViewModel>> GetProductListByCategoryID(int id);
        //void Remove(int productid);
        //string[] GetProductListAlike(string productname);
        //string GetProducts2(int id);
        IEnumerable<Areas.EgsOperations.Models.EgsProductViewModel> GetProductListAlike(string productname, string prodCategory);
        Task<EgsProduct> GetSingleProductByMerchantId(int id);
        Task<AirtimeProductList> GetAirtimeList();
        Task<DataProductList> GetDataList();
        Task<List<EgsProduct>> GetAllAutoRegProducts();
        Task<List<EgsProduct>> GetAllProductsByPrdCart(int id);

    }
}
