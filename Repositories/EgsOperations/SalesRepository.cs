using Microsoft.EntityFrameworkCore;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class SalesRepository : Repository<EgsSales>, ISalesRepository
    {
        ApplicationDBContext _context;

        public SalesRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EgsSales>> GetSalesByDateRange(DateTime start, DateTime end)
        {
            List<EgsSales> salesData = null;
            try
            {
                salesData = await Context.EGS_Sales.Where(x => x.DateCreated >= start && x.DateCreated <= end)
                            .ToListAsync();



                    return salesData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    





    }
}
