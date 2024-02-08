using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Areas.SysSetup.Models;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class ProductItemRepository : Repository<EgsProductItem>, IProductItemRepository
    {
        ApplicationDBContext _context;

        public ProductItemRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        //
        public async Task<EgsProductItem> GetProductItems(int id)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemID == id)
                            .Include(a => a.Product)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<EgsProductItem>> GetProductItemCategories()
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemCategory != null)
                            .Include(a => a.Product)
                            .AsNoTracking()
                            .OrderBy(g => g.ProductItemCategory)
                            .ToListAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EgsProductItem> GetProductItemCategoriesByCat(string itemCat)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemCategory == itemCat)
                            .Include(a => a.Product)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProductItem> GetProductItem(int id)
        {
            try
            {
                //var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemID == id)

                var itemDetails = await Context.EGS_ProductItem.Where(p => p.Product.ProductID == id)
                         .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProductItem> GetProductItemByItemCode(string itemCode)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemCode == itemCode)
                            .Include(p => p.Product)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsProductItem> GetSingleProductItem(string itemCode, int productId)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem
                            .Where(p => (p.ProductItemCode == itemCode || p.ProductItemName== itemCode) && p.Product.ProductID == productId )
                            .Include(p => p.Product)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EgsProductItem> GetSingleProductByItem(int pItemId)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem
                            .Where(p => p.ProductItemID == pItemId)
                            .Include(p => p.Product)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<IEnumerable<EgsProductItemViewModel>> GetProductitemCategory()
        //{
        //    try
        //    {
        //        var results = Context.EGS_ProductItem
        //                .FromSql("SELECT DISTINCT ProductItemCategory,ProductID, COUNT(*) FROM EGS_ProductItem " +
        //                "GROUP BY ProductItemCategory,ProductID")
        //                .Select(u => new EgsProductItemViewModel
        //                { 
        //                    ProductID=u.Product.ProductID, 
        //                    ProductItemCategory = u.ProductItemCategory
        //                }).ToListAsync();

        //        return await results;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        public async Task<List<EgsProductItem>> GetAllProductItems()
        {
            try
            {
                var allProductItems = await Context.EGS_ProductItem
                            .Include(x => x.Product)
                            .AsNoTracking()
                            .ToListAsync();
                return allProductItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsProductItem>> GetProductProcessingItems()
        {
            try
            {
                var allProductItems = await Context.EGS_ProductItem
                            .Where(p => p.ProductItemName.Contains("Transaction Fee"))
                            .Include(x => x.Product)
                            .AsNoTracking()
                            .ToListAsync();
                return allProductItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsProductItem>> GetItemsByProductID(int id)
        {
            try
            {
            var allItems = await Context.EGS_ProductItem.Where(p => p.Product.ProductID == id)
                      .Include(x => x.Product)
                      .AsNoTracking()
                      .ToListAsync();
                return allItems;


                //var results = Context.EGS_ProductItem 
                //    .FromSql(" select ProductItemID,ProductID,ProductItemName from EGS_ProductItem where IsActive = 1" +
                //    " AND ProductID = {0} ", id)
                //      .Select(u => new EgsProductItem 
                //      {
                //          //ProductID = u.Product.ProductID,
                //          ProductItemName = u.ProductItemName,
                //          ProductItemID = u.ProductItemID
                //      })
                //     .ToListAsync();

                //        return await results ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProductItem> GetProductItemsByProdId(int id) // Sending in the ProductItemID id
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemID == id)
                            .Include(x => x.Product)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<EgsProductItem> GetProducttransactionfee(int prdid)  
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.Product.ProductID == prdid && p.Istransactionfee==true)
                            .Include(x => x.Product)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsProductItem> GetProductItemByItemName(string itemName)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItem.Where(p => p.ProductItemName == itemName)
                            .Include(p => p.Product)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
