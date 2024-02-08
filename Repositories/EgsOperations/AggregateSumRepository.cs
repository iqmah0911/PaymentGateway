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
    public class AggregateSumRepository : Repository<EgsAggregateSum>, IAggregateSumRepository
    { 
            ApplicationDBContext _context;

            public AggregateSumRepository(ApplicationDBContext context) : base(context)
            {
                _context = context;
            }


        public async Task<EgsAggregateSum> GetAggregateSum()
        {
            try
            {
                var remDetails = await Context.EGS_AggregateSum
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                return remDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}
