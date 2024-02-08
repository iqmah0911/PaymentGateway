//using PaymentGateway.Repositories;
using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class UserTypeRepository : Repository<SysUserType>, IUserTypeRepository
    {
        ApplicationDBContext _context;

        public UserTypeRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
