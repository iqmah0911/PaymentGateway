using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.EgsOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.EgsOperations
{
    public class AuthFactorRepository : Repository<EgsAuthFactor>, IAuthFactorRepository
    {
        ApplicationDBContext _context;

        public AuthFactorRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
         

    }
}
