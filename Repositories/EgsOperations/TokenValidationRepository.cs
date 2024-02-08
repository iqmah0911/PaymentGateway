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
    public class TokenValidationRepository : Repository<EgsTokenValidation>, ITokenValidationRepository
    {
        ApplicationDBContext _context;


        public TokenValidationRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }



        public async Task<EgsTokenValidation> GetToken(string name)
        {
            try
            {
                var TokenDetails = await Context.EGS_TokenValidation.Where(p => p.Provider == name)
                            .FirstOrDefaultAsync();

                return TokenDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
