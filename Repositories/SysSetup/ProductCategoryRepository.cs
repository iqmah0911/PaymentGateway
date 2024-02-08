using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class ProductCategoryRepository : Repository<EgsProductCategory>, IProductCategoryRepository
    {
        ApplicationDBContext _context;

        public ProductCategoryRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
        //ProductCategory



        public async Task<EgsProductCategory> GetProductCategory(int id)
        {
            try
            {
                var prodCategoryDetails = await Context.EGS_ProductCategories.Where(p => p.ProductCategoryID == id)
                            //.AsNoTracking()
                            .SingleOrDefaultAsync();

                return prodCategoryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<EgsProductCategory> GetProductCategoryByName(string CategoryName)
        {
            try
            {
                var prodCategoryDetails = await Context.EGS_ProductCategories.Where(p => p.ProductCategoryName == CategoryName)
                            //.AsNoTracking()
                            .FirstOrDefaultAsync();

                return prodCategoryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EgsProductCategory>> GetAllProductCategory()
        {
            try
            {
                var allProductCategory = await Context.EGS_ProductCategories.Where(x=>x.ProductCategoryName!="Transaction")
                            .AsNoTracking()
                            .ToListAsync();

                return allProductCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<EgsProductCategory> GetAllForDropdown()
        {
            return Context.EGS_ProductCategories.ToList();
        }


        public List<EgsProductCategory> GetAllForDropdownList()
        {
            return Context.EGS_ProductCategories.ToList();
        }

    }


}
