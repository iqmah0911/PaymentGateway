using PaymentGateway21052021.Data;
using PaymentGateway21052021.Models;
using PaymentGateway21052021.Repositories.SysSetup.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Repositories.SysSetup
{
    public class UserContactInfoRepository : Repository<SysUserContactInfo>, IUserContactInfoRepository
    {
        ApplicationDBContext _context;

        public UserContactInfoRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
    
    
}
