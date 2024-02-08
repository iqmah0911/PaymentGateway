using Microsoft.EntityFrameworkCore;
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
    public class ProductItemRateRepository : Repository<EgsProductItemRate>, IProductItemRateRepository
    {
        ApplicationDBContext _context;

        public ProductItemRateRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EgsProductItemRate> GetProductItemRates(int id)
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItemRate.Where(p => p.ProductItemRateID == id)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EgsProductItemRate>> GetAllProductItemRates()
        {
            try
            {
                var allProductItemRates = await Context.EGS_ProductItemRate
                            .Include(x => x.ProductItem)
                            .AsNoTracking()
                            .ToListAsync();
                return allProductItemRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<List<EgsProductItemRate>> GetItemRateByProductID(int id)
        {
            try
            {
                var ItemRates = await Context.EGS_ProductItemRate.Where(pRate => pRate.ProductItem.Product.ProductID == id )
                            .Include(x => x.ProductItem)
                            .ThenInclude(p => p.Product)
                            .AsNoTracking()
                            .ToListAsync();
                return ItemRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<EgsProductItemRate> GetItemRateByProductItemID(int id)
        {
            try
            {
                var ItemRates = await Context.EGS_ProductItemRate.Where(pRate => pRate.ProductItem.ProductItemID == id)
                            .Include(x => x.ProductItem)
                            .ThenInclude(p => p.Product)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();
                return ItemRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProductItemRate> GetItemRateByProductItemIDAPI(int id)
        {
            try
            {
                var ItemRates = await Context.EGS_ProductItemRate
                            .Where(pRate => pRate.ProductItem.ProductItemID == id) 
                            .Include(x => x.ProductItem)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
                return ItemRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EgsProductItemRate> GetProductItemsByProdItemId(int id) // Sending in the product id
        {
            try
            {
                var itemDetails = await Context.EGS_ProductItemRate.Where(p => p.ProductItemRateID == id)
                            .Include(x => x.ProductItem)
                            .AsNoTracking()
                            .SingleOrDefaultAsync();

                return itemDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<EgsProductItemRate>> GetAllProductItemRatesByItemId(int id)
        {
            try
            {
                var allProductItemRates = await Context.EGS_ProductItemRate.Where(p => p.ProductItem.ProductItemID == id)
                            .Include(x => x.ProductItem)
                            .AsNoTracking()
                            .ToListAsync();
                return allProductItemRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
