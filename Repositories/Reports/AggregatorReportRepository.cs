
using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.Reports.Interfaces
{
    public class AggregatorReportRepository : Repository<EgsSales>, IAggregatorReportRepository
    {
        ApplicationDBContext _context;

        public AggregatorReportRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EgsSales>> GetSalesByDateRange(DateTime start, DateTime end, int id)
        {
            IEnumerable<EgsSales> salesData = null;
            try
            {
                salesData = await Context.EGS_Sales.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.CreatedBy == id ) //||(!x.ProductItem.ProductItemName.Contains("Transaction Fee")))
                     .Include(p=>p.Product)
                     .Include(pr => pr.ProductItem)
                    .ToListAsync();

                return salesData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EgsSales>> GetAllSalesByDateRange(DateTime start, DateTime end, List<string> Id) //int id
        {
            List<EgsSales> salesData = null;
            try
            {
                //salesData = await Context.EGS_Sales.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.CreatedBy == id || (!x.ProductItem.ProductItemName.Contains("Transaction Fee")))
                //     .Include(p => p.Product)
                //     .Include(pr => pr.ProductItem) 
                //    .ToListAsync();
                List<EgsSales> result=new List<EgsSales>();
                foreach (var vid in Id)
                {
                   salesData = await Context.EGS_Sales.Where(x => x.DateCreated.Date >= start && x.DateCreated.Date <= end && x.CreatedBy == Convert.ToInt32(vid))   //|| (!x.ProductItem.ProductItemName.Contains("Transaction Fee")))
                       .Include(p => p.Product)
                       .Include(pr => pr.ProductItem) 
                      .ToListAsync();
                    foreach (var data in salesData)
                    {
                        result.Add(data);
                    } 
                }

                return result;
                //return salesData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         




    }
}
