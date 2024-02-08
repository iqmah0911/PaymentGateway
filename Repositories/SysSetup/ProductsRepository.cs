//using PaymentGateway.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Areas.EgsOperations.Models;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class ProductsRepository : Repository<EgsProduct>, IProductsRepository
    {
        ApplicationDBContext _context;

        public ProductsRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsProduct> GetProducts(int id)
        {
            try
            {
                var productDetails = await Context.EGS_Products
                    .Include(m => m.Merchant)
                       .Include(m => m.ProductCategories)
                    .Where(p => p.ProductID == id)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();

                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProductItem> GetProductExtension(int id)
        {
            try
            {
                var productExt = await Context.EGS_ProductItem.Where(p => p.ProductItemID == id)
                            //.AsNoTracking()
                            .Include(x => x.Product)
                            .SingleOrDefaultAsync();

                return productExt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsProduct>> GetAllProducts()
        {
            try
            {
                var allProduct = await Context.EGS_Products
                            .AsNoTracking()
                            .ToListAsync();

                return allProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsProduct>> GetProductList()
        {
            try
            {
                var allProduct = await Context.EGS_Products
                    .Include(pc => pc.ProductCategories)
                            .AsNoTracking()
                            .ToListAsync();

                return allProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IEnumerable<Areas.EgsOperations.Models.EgsProductViewModel> GetProductListAlike(string productname, string prodCategory)
        {
            try
            {
                if (!string.IsNullOrEmpty(prodCategory) && prodCategory != "0")
                {
                    var resultsWithCategory = Context.EGS_Products
                        //.Where(u => u.Name.StartsWith(name) && !u.Deleted && u.AppearInSearch)
                        .FromSql("select ProductName,ProductID,ProductCategoryID,InvoiceModeID " +
                        "from EGS_Products where IsActive = 1 AND ProductName like {0} AND ProductCategoryID = {1}", productname + "%", prodCategory)
                        //.Where(!u.Deleted && u.AppearInSearch)
                        //.OrderByDescending(u => u.Verified)
                        //.ThenBy(u => u.DateAdded) // Added to prevent duplication of results in different pages
                        //.Skip(page * recordsInPage)
                        //.Take(recordsInPage)
                        .Select(u => new Areas.EgsOperations.Models.EgsProductViewModel
                        {
                            ProductName = u.ProductName,
                            ProductID = u.ProductID,
                            InvoiceModeID =u.InvoiceModeID,
                            ProductCategoryID = u.ProductCategories.ProductCategoryID
                        }).ToList();

                    return resultsWithCategory;
                }
                else
                {
                    var results = Context.EGS_Products
                        .FromSql("select ProductName,ProductID,ProductCategoryID,InvoiceModeID " +
                        "from EGS_Products where IsActive = 1 AND ProductName like {0} ", productname + "%")
                        .Select(u => new Areas.EgsOperations.Models.EgsProductViewModel
                        {
                            ProductName = u.ProductName,
                            ProductID = u.ProductID,
                            InvoiceModeID=u.InvoiceModeID,
                            ProductCategoryID = u.ProductCategories.ProductCategoryID
                        }).ToList();
                    return results;
                }
                //var blogs3 = ctx.Blogs.FromSqlRaw("SELECT * FROM Blogs AS b WHERE b.Name LIKE {0} + '%' OR b.Name LIKE {1} + '%'", pattern1, pattern2).ToList();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IEnumerable<HomeStoreModel> GetProducts(string pcode)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost/PaymentGatewayPublish/");
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var response = client.GetAsync("api/Product/GetProducts?productname=" + pcode).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var res = response.Content.ReadAsAsync<IEnumerable<HomeStoreModel>>().Result;
        //        return res;
        //    }

        //    return null;
        //}

        public async Task<EgsProduct> GetProductCategoryById(int id) // Sending in the ProductID 
        {
            try
            {
                var itemDetails = await Context.EGS_Products.Where(p => p.ProductID == id)
                            .Include(x => x.ProductCategories)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EgsProductViewModel>> GetProductListByCategoryID(int id)
        {
            try
            {
                var results = Context.EGS_Products.Where(u => u.IsActive == true && u.ProductCategories.ProductCategoryID==id)
                        //.FromSql("select ProductName,ProductID,ProductCategoryID from EGS_Products where IsActive = 1 AND ProductCategoryID = {0} ", id)
                    .Include(u=>u.ProductCategories)    
                    .Select(u => new EgsProductViewModel
                        {
                            ProductName = u.ProductName,
                            ProductID = u.ProductID,
                            ProductCategoryID = u.ProductCategories.ProductCategoryID,
                            ProductCategory = u.ProductCategories.ProductCategoryName
                        }).ToListAsync();

                return await results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProduct> GetProductByName(string name)  
        {
            try
            {
                var itemDetails = await Context.EGS_Products.Where(p => p.ProductName == name)
                            .Include(x => x.ProductCategories)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsProductViewModel>> GetProductNasarawaByCategoryID()
        {
            try
            {
                var results = Context.EGS_Products.Where(u=>u.IsActive==true && (u.ProductID==36 || u.ProductID==37))
                        //.FromSql("select ProductName,ProductID,ProductCategoryID from EGS_Products where IsActive = 1 AND ProductID = 36 OR ProductID = 37")
                    .Include(u=>u.ProductCategories)    
                    .Select(u => new EgsProductViewModel
                        {
                            ProductName = u.ProductName,
                            ProductID = u.ProductID,
                            ProductCategoryID = u.ProductCategories.ProductCategoryID,
                            ProductCategory = u.ProductCategories.ProductCategoryName
                        }).ToListAsync();

                return await results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         

        public async Task<List<EgsProduct>> GetProductsByMerchantId(int id)
        {
            try
            {
                var productDetails = await Context.EGS_Products.Where(p => p.Merchant.MerchantID == id)
                            //.AsNoTracking()
                            .ToListAsync();

                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsProduct> GetSingleProductByMerchantId(int id)
        {
            try
            {
                var productDetails = await Context.EGS_Products.Where(p => p.Merchant.MerchantID == id)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();

                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<EgsProduct> GetProductInvoiceModeById(int id) // Sending in the ProductID 
        {
            try
            {
                var itemDetails = await Context.EGS_Products.Where(p => p.ProductID == id)
                            .Include(x => x.ProductCategories)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProduct> GetTransactionfeebyProductCode(string code)
        {
            try
            {
                var itemDetails = await Context.EGS_Products.Where(p => p.ProductCode.Equals(code))
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public async Task<AirtimeProductList> GetAirtimeList()
        {
            try
            {
                var initmodel = new AirtimeProductList();
                var result = new List<AirtimeProducts>();
                var items = await Context.EGS_Products.Where(p => p.InvoiceModeID.Equals(1) && p.ProductCode != "TRF001")
                            //.AsNoTracking()
                            .ToListAsync();
                foreach (var item in items)
                {
                    result.Add(new AirtimeProducts
                    {
                        ProductID = item.ProductID,
                        ProductModeID = item.InvoiceModeID,
                        ProductName = item.ProductName
                    });
                }
                initmodel.AirtimeList = result;


                return initmodel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<DataProductList> GetDataList()
        {
            try
            {
                var result = new DataProductList();
                var dlist = new List<DataProducts>();
                var items = await Context.EGS_Products.Where(p => p.InvoiceModeID.Equals(5))
                            //.AsNoTracking()
                            .ToListAsync();
                foreach (var item in items)
                {
                    dlist.Add(new DataProducts
                    {
                        ProductID = item.ProductID,
                        ProductModeID = item.InvoiceModeID,
                        ProductName = item.ProductName,
                        ServiceCode = "BDA"
                    });
                }
                result.DataProdList = dlist;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsProduct>> GetAllAutoRegProducts()
        {
            try
            {
                var allProduct = await Context.EGS_Products.Where(x => x.InvoiceModeID == 3)
                            .AsNoTracking()
                            .ToListAsync();

                return allProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<List<EgsProduct>> GetAllProductsByPrdCart(int id)
        {
            try
            {
                var allProduct = await Context.EGS_Products
                    .Include(pc => pc.ProductCategories)
                    .Where(p => p.ProductCategories.ProductCategoryID == id && p.ProductCategories.ProductCategoryName != "Ticket")
                            .AsNoTracking()
                            .ToListAsync();

                return allProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }











    }
}
