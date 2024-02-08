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
    public class UserTokenRepository : Repository<EgsUserToken>, IUserTokenRepository
    {
        ApplicationDBContext _context;
 

        public UserTokenRepository(ApplicationDBContext context) : base(context)
        {
          _context = context;   
        }


        public async Task<EgsUserToken> GetPin(int id)
        {
            try
            {
                var pinDetails = await Context.EGS_UserToken.Where(p => p.UserID == id) 
                            .FirstOrDefaultAsync();

                return pinDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
